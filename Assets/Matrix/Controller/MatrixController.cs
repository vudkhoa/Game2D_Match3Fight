namespace Controller.Matrix
{
    using Controller.Queue;
    using Cotroller;
    using CustomData;
    using CustomUtils;
    using Model.Matrix;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using View.Matrix;

    public enum Direction
    {
        Left = 1,
        Right = 2,
        Up = 3,
        Down = 4,
        None = 0,
    }

    public class MatrixController : SingletonMono<MatrixController>
    {
        [Header("Matrix Draw")]
        public MatrixElementView MatrixElement;
        public RectTransform MatrixParent;

        [Header("Matrix Data")]
        public MatrixElementModel[,] MatrixElementModelList;
        public Vector2Int CurrentPos;
        private Vector2Int MatrixSize;

        private Vector3 startPos;
        private Vector3 endPos;
        private bool CheckMatch = false; 

        private void Start()
        {
            this.CreateMatrix();
        }

        private void Update()
        {
            Direction dir = GetDirection();
            if (dir != Direction.None && this.CurrentPos != new Vector2(-1, -1)) 
            {
                Vector2Int newPos = this.CurrentPos;

                switch (dir)
                {
                    case Direction.Left:
                        newPos.y -= 1;
                        break;
                    case Direction.Right:
                        newPos.y += 1;
                        break;
                    case Direction.Up:
                        newPos.x -= 1;
                        break;
                    case Direction.Down:
                        newPos.x += 1;
                        break;
                }

                if (!MatrixElementModelList[this.CurrentPos.x, this.CurrentPos.y].IsMatch
                    && !MatrixElementModelList[newPos.x, newPos.y].IsMatch)
                {
                    Debug.Log("Swap: " + this.CurrentPos + " - " + newPos);
                    StartCoroutine(this.MatrixElementSwap(this.CurrentPos, newPos));
                    MatrixElementType currentType = MatrixElementModelList[this.CurrentPos.x, this.CurrentPos.y].MatrixElementType;
                    MatrixElementType newType = MatrixElementModelList[newPos.x, newPos.y].MatrixElementType;
                    CheckMatch = false;
                    StartCoroutine(this.CheckingForSwap(this.CurrentPos, newPos));
                }
                this.ResetCurrentPos();
            }
        }

        private IEnumerator CheckingForSwap(Vector2Int curPos, Vector2Int newPos)
        {
            yield return new WaitForSeconds(0.45f);
            bool tmpCheck = false;
            this.CheckingMatch(curPos);
            tmpCheck = CheckMatch;
            this.CheckingMatch(newPos);
            tmpCheck = CheckMatch || tmpCheck;
            yield return new WaitForSeconds(0.05f);
            if (!tmpCheck)
            {
                Debug.Log("Not Match");
                StartCoroutine(this.MatrixElementSwap(curPos, newPos));
            }
            else
            {
                ReFlowDown();
            }
        }

        private IEnumerator ShuffleMap()
        {
            yield return new WaitForSeconds(1f);
            List<MatrixElementType> elementTypes = new List<MatrixElementType>();
            
            for (int i = MatrixSize.x - 1; i >= 0; --i)
            {
                for (int j = MatrixSize.y - 1; j >= 0; --j)
                {
                    if (!MatrixElementModelList[i, j].IsMatch)
                    {
                        elementTypes.Add(MatrixElementModelList[i, j].MatrixElementType);
                    }
                }
            }

            bool checkCount = false;
            int count = 0;

            foreach (MatrixElementType m in elementTypes)
            {
                count = 0;
                foreach (MatrixElementType n in elementTypes)
                {
                    if (m == n)
                    {
                        count++;
                        if (count >= 3)
                        {
                            checkCount = true;
                            break;
                        }
                    }
                }
            }
            if (checkCount)
            {
                ShuffleList(elementTypes);

                int index = -1;
                for (int i = MatrixSize.x - 1; i >= 0; --i)
                {
                    for (int j = 0; j < MatrixSize.y; ++j)
                    {
                        index++;
                        if (index < elementTypes.Count)
                        {
                            MatrixElementModelList[i, j].SetType(elementTypes[index]);
                            MatrixElementModelList[i, j].SetMatch(false);
                        }
                        else
                        {
                            MatrixElementModelList[i, j].SetMatch(true);
                        }
                    }
                }
                StartCoroutine(ReCheckingMatch());
            }
            else
            {
                CoreGamePlayController.Instance.StartPhase2();
            }
        }

        private void ShuffleList<T>(List<T> list)
        {
            System.Random random = new System.Random();
            int n = list.Count;

            for (int i = n - 1; i > 0; i--)
            {
                int j = random.Next(0, i + 1);

                T temp = list[i];
                list[i] = list[j];
                list[j] = temp;
            }
        }

        private bool CheckPosInvalid(Vector2Int pos)
        {
            return (pos.x >= 0 && pos.x < MatrixSize.x) && 
                   (pos.y >= 0 && pos.y < MatrixSize.y) &&
                   !MatrixElementModelList[pos.x, pos.y].IsMatch;
        }

        private bool CheckMap()
        {
            for (int i = MatrixSize.x - 1; i >= 0; --i)
            {
                for (int j = MatrixSize.y - 1; j >= 0; --j)
                {
                    if (!this.MatrixElementModelList[i, j].IsMatch)
                    {
                        MatrixElementType type = this.MatrixElementModelList[i, j].MatrixElementType;
                        Vector2Int posMatch = new Vector2Int(-1, -1);
                        posMatch.x = i;
                        posMatch.y = j + 1;
                        if 
                        (
                            CheckPosInvalid(posMatch) && 
                            this.MatrixElementModelList[posMatch.x, posMatch.y].MatrixElementType == type &&
                            (j + 2 >= 0 && j + 2 < MatrixSize.y) &&
                            !this.MatrixElementModelList[i, j + 2].IsMatch
                        )
                        {
                            //Debug.Log(1);
                            Vector2Int posCheck1 = new Vector2Int(i - 1, j + 2);
                            Vector2Int posCheck2 = new Vector2Int(i + 1, j + 2);
                            Vector2Int posCheck3 = new Vector2Int(i, j + 3);
                            if ((CheckPosInvalid(posCheck1) && this.MatrixElementModelList[posCheck1.x, posCheck1.y].MatrixElementType == type) ||
                                (CheckPosInvalid(posCheck2) && this.MatrixElementModelList[posCheck2.x, posCheck2.y].MatrixElementType == type) ||
                                (CheckPosInvalid(posCheck3) && this.MatrixElementModelList[posCheck3.x, posCheck3.y].MatrixElementType == type))
                            {
                                return true;
                            }
                        }

                        posMatch.x = i + 1;
                        posMatch.y = j;
                        if (
                            CheckPosInvalid(posMatch) && 
                            this.MatrixElementModelList[posMatch.x, posMatch.y].MatrixElementType == type &&
                            (i + 2 >= 0 && i + 2 < MatrixSize.x) &&
                            !this.MatrixElementModelList[i + 2, j].IsMatch
                        )
                        {
                            //Debug.Log(2);
                            Vector2Int posCheck1 = new Vector2Int(i + 2, j - 1);
                            Vector2Int posCheck2 = new Vector2Int(i + 2, j + 1);
                            Vector2Int posCheck3 = new Vector2Int(i + 3, j);
                            if ((CheckPosInvalid(posCheck1) && this.MatrixElementModelList[posCheck1.x, posCheck1.y].MatrixElementType == type) ||
                                (CheckPosInvalid(posCheck2) && this.MatrixElementModelList[posCheck2.x, posCheck2.y].MatrixElementType == type) ||
                                (CheckPosInvalid(posCheck3) && this.MatrixElementModelList[posCheck3.x, posCheck3.y].MatrixElementType == type))
                            {
                                return true;
                            }
                        }

                        posMatch.x = i;
                        posMatch.y = j + 2;
                        if (
                            CheckPosInvalid(posMatch) && 
                            this.MatrixElementModelList[posMatch.x, posMatch.y].MatrixElementType == type &&
                            (j + 1 >= 0 && j + 1 < MatrixSize.y) &&
                            !this.MatrixElementModelList[i, j + 1].IsMatch
                        )
                        {
                            //Debug.Log(3);
                            Vector2Int posCheck1 = new Vector2Int(i - 1, j + 1);
                            Vector2Int posCheck2 = new Vector2Int(i + 1, j + 1);
                            if ((CheckPosInvalid(posCheck1) && this.MatrixElementModelList[posCheck1.x, posCheck1.y].MatrixElementType == type) ||
                                (CheckPosInvalid(posCheck2) && this.MatrixElementModelList[posCheck2.x, posCheck2.y].MatrixElementType == type))
                            {
                                return true;
                            }
                        }

                        posMatch.x = i + 2;
                        posMatch.y = j;
                        if (
                            CheckPosInvalid(posMatch) && 
                            this.MatrixElementModelList[posMatch.x, posMatch.y].MatrixElementType == type &&
                            (i + 1 >= 0 && i + 1 < MatrixSize.x) &&
                            !this.MatrixElementModelList[i + 1, j].IsMatch  
                        )
                        {
                            Vector2Int posCheck1 = new Vector2Int(i + 1, j - 1);
                            Vector2Int posCheck2 = new Vector2Int(i + 1, j + 1);
                            if ((CheckPosInvalid(posCheck1) && this.MatrixElementModelList[posCheck1.x, posCheck1.y].MatrixElementType == type) ||
                                (CheckPosInvalid(posCheck2) && this.MatrixElementModelList[posCheck2.x, posCheck2.y].MatrixElementType == type))
                            {
                                return true;
                            }
                        }


                        posMatch.x = i;
                        posMatch.y = j + 3;
                        if (
                             CheckPosInvalid(posMatch) &&
                             this.MatrixElementModelList[posMatch.x, posMatch.y].MatrixElementType == type &&
                             (j + 1 >= 0 && j + 1 < MatrixSize.y) &&
                             (j + 2 >= 0 && j + 2 < MatrixSize.y) &&
                             !this.MatrixElementModelList[i, j + 1].IsMatch &&
                             !this.MatrixElementModelList[i, j + 2].IsMatch &&
                             this.MatrixElementModelList[i, j + 2].MatrixElementType == type
                        )
                        {
                            return true;
                        }

                        posMatch.x = i + 3;
                        posMatch.y = j;
                        if (
                             CheckPosInvalid(posMatch) &&
                             this.MatrixElementModelList[posMatch.x, posMatch.y].MatrixElementType == type &&
                             (i + 1 >= 0 && i + 1 < MatrixSize.x) &&
                             (i + 2 >= 0 && i + 2 < MatrixSize.x) &&
                             !this.MatrixElementModelList[i + 1, j].IsMatch &&
                             !this.MatrixElementModelList[i + 2, j].IsMatch &&
                             this.MatrixElementModelList[i + 2, j].MatrixElementType == type
                        )
                        {
                            return true;
                        }

                        if (
                            CheckPosInvalid(new Vector2Int(i + 1, j + 1)) &&
                            CheckPosInvalid(new Vector2Int(i + 1, j + 2)) &&
                            CheckPosInvalid(new Vector2Int(i + 1, j)) &&
                            this.MatrixElementModelList[i + 1, j + 1].MatrixElementType == type &&
                            this.MatrixElementModelList[i + 1, j + 2].MatrixElementType == type
                        )
                        {
                            return true;
                        }

                        if (
                            CheckPosInvalid(new Vector2Int(i - 1, j + 1)) &&
                            CheckPosInvalid(new Vector2Int(i - 1, j + 2)) &&
                            CheckPosInvalid(new Vector2Int(i - 1, j)) &&
                            this.MatrixElementModelList[i - 1, j + 1].MatrixElementType == type &&
                            this.MatrixElementModelList[i - 1, j + 2].MatrixElementType == type
                        )
                        {
                            return true;
                        }

                        if (
                            CheckPosInvalid(new Vector2Int(i + 1, j - 1)) &&
                            CheckPosInvalid(new Vector2Int(i + 2, j - 1)) &&
                            CheckPosInvalid(new Vector2Int(i, j - 1)) &&
                            this.MatrixElementModelList[i + 1, j - 1].MatrixElementType == type &&
                            this.MatrixElementModelList[i + 2, j - 1].MatrixElementType == type
                        )
                        {
                            return true;
                        }

                        if (
                            CheckPosInvalid(new Vector2Int(i + 1, j + 1)) &&
                            CheckPosInvalid(new Vector2Int(i + 2, j + 1)) &&
                            CheckPosInvalid(new Vector2Int(i, j + 1)) &&
                            this.MatrixElementModelList[i + 1, j + 1].MatrixElementType == type &&
                            this.MatrixElementModelList[i + 2, j + 1].MatrixElementType == type
                        )
                        {
                            return true;
                        }

                    }
                }
            }
            
            return false;
        }

        private IEnumerator ReCheckingMatch()
        {
            yield return new WaitForSeconds(1.05f);
            bool isMatch = false;
            for (int i = 0; i < MatrixSize.x; ++i)
            {
                for (int j = 0; j < MatrixSize.y; ++j)
                {
                    if (!this.MatrixElementModelList[i, j].IsMatch)
                    {
                        CheckMatch = false;
                        CheckingMatch(new Vector2Int(i, j));
                        isMatch = CheckMatch || isMatch;
                    }
                }
            }
            if (isMatch)
            {
                ReFlowDown();
            }
            else
            {
                if (!this.CheckMap())
                {
                    Debug.Log("Not CheckMap");
                    StartCoroutine(this.ShuffleMap());
                }
            }
        }

        private void ReFlowDown()
        {
            for (int i = 0; i < DataManager.Instance.MatrixData.InformationMatrix.Size.y; ++i)
            {
                this.FlowDown(i);
            }
            StartCoroutine(ReCheckingMatch());
        }

        private IEnumerator SwapMatrixElementModel(Vector2Int pos1, Vector2Int pos2, Vector2 posAnim)
        {
            this.MatrixElementModelList[pos1.x, pos1.y].MatrixElementView.PlayMoveAnimation(posAnim);
            yield return new WaitForSeconds(0.4f);
            MatrixElementModelList[pos1.x, pos1.y].SetMatch(true);
            MatrixElementModelList[pos2.x, pos2.y].SetType(MatrixElementModelList[pos1.x, pos1.y].MatrixElementType);
            MatrixElementModelList[pos2.x, pos2.y].SetMatch(false);
        }

        private void ResetCurrentPos()
        {
            this.CurrentPos = new Vector2Int(-1, -1);
        }

        private IEnumerator MatrixElementSwap(Vector2Int from, Vector2Int to)
        {
            Debug.Log("Swap: " + from + " - " + to);
            Vector2 toPos = MatrixElementModelList[to.x, to.y].MatrixElementView.GetComponent<RectTransform>().anchoredPosition;
            Vector2 fromPos = MatrixElementModelList[from.x, from.y].MatrixElementView.GetComponent<RectTransform>().anchoredPosition;
            MatrixElementModelList[from.x, from.y].MatrixElementView.PlaySwapAnimation(toPos);
            MatrixElementModelList[to.x, to.y].MatrixElementView.PlaySwapAnimation(fromPos);
            yield return new WaitForSeconds(0.4f);
            MatrixElementType tmpType = MatrixElementModelList[from.x, from.y].MatrixElementType;
            MatrixElementModelList[from.x, from.y].SetType(MatrixElementModelList[to.x, to.y].MatrixElementType);
            MatrixElementModelList[to.x, to.y].SetType(tmpType);
        }

        // Check Match
        public void CheckingMatch(Vector2Int pos)
        {
            CheckMatch = false;
            // Row
            // Get Data
            MatrixElementType type = MatrixElementModelList[pos.x, pos.y].MatrixElementType;

            int iRow, jRow;
            iRow = 0;
            jRow = 0;
            // Check Left
            for (iRow = pos.y; iRow >= 0; iRow--)
            {
                if (MatrixElementModelList[pos.x, iRow].MatrixElementType != type || MatrixElementModelList[pos.x, iRow].IsMatch)
                {
                    break;
                }
            }
            iRow += 1;

            // Check Right
            for (jRow = pos.y; jRow < MatrixSize.x; jRow++)
            {
                if (MatrixElementModelList[pos.x, jRow].MatrixElementType != type || MatrixElementModelList[pos.x, jRow].IsMatch)
                {
                    break;
                }
            }
            jRow -= 1;

            // Delete
            if (jRow - iRow + 1 >= 3 && iRow >= 0 && jRow < MatrixSize.x)
            {
                CheckMatch = true;
                for (int i = iRow; i <= jRow; ++i)
                {
                    if (i != pos.y)
                    {
                        MatrixElementModelList[pos.x, i].SetMatch(true, true);
                    }
                }
            }

            // Column
            int iColumn, jColumn;
            iColumn = 0;
            jColumn = 0;

            // Check up
            for (iColumn = pos.x; iColumn >= 0; iColumn--)
            {
                if (MatrixElementModelList[iColumn, pos.y].MatrixElementType != type || (MatrixElementModelList[iColumn, pos.y].IsMatch && iColumn != pos.x))
                {
                    break;
                }
            }
            iColumn += 1;

            // Check down
            for (jColumn = pos.x; jColumn < MatrixSize.y; jColumn++)
            {
                if (MatrixElementModelList[jColumn, pos.y].MatrixElementType != type || (MatrixElementModelList[jColumn, pos.y].IsMatch && jColumn != pos.x))
                {
                    break;
                }
            }
            jColumn -= 1;

            if (jColumn - iColumn + 1 >= 3 && iColumn >= 0 && jColumn < MatrixSize.y)
            {
                CheckMatch = true;
                for (int i = iColumn; i <= jColumn; ++i)
                {
                    if (i != pos.x)
                    {
                        MatrixElementModelList[i, pos.y].SetMatch(true, true);
                    }
                }
            }

            if (CheckMatch)
            {
                MatrixElementModelList[pos.x, pos.y].SetMatch(true, true);
            }

            int sum = (jColumn - iColumn + 1) + (jRow - iRow + 1) - 1;
            int p = 0;
            if (sum == 3 || sum == 4)
            {
                p = 1;
            }
            else if (sum >= 5)
            {
                p = 3;
            }

            if (CheckMatch)
            {
                QueueController.Instance.PlusCount(type, p);
            }
        }

        // Draw Matrix 6x6
        private void CreateMatrix()
        {
            // Init CurrentPos
            this.CurrentPos = new Vector2Int(-1, -1);

            // Get Data Matrix from DataManager
            MatrixSize = DataManager.Instance.MatrixData.InformationMatrix.Size;
            int directionX = DataManager.Instance.MatrixData.InformationMatrix.Direction.x;
            int directionY = DataManager.Instance.MatrixData.InformationMatrix.Direction.y;
            float widthMatrix = DataManager.Instance.MatrixData.InformationMatrix.Width;
            float heightMatrix = DataManager.Instance.MatrixData.InformationMatrix.Height;
            float paddingX = DataManager.Instance.MatrixData.InformationMatrix.Padding.x;
            float paddingY = DataManager.Instance.MatrixData.InformationMatrix.Padding.y;
            float sizeElement = DataManager.Instance.MatrixData.InformationElement.Size;
            float spacingX = DataManager.Instance.MatrixData.InformationMatrix.Spacing.x;
            float spacingY = DataManager.Instance.MatrixData.InformationMatrix.Spacing.y;
            float initPosX = -directionX * (widthMatrix / 2) +
                             paddingX * directionX + (sizeElement / 2) * directionX;
            float initPosY = -directionY * (heightMatrix / 2) +
                             paddingY * directionY + (sizeElement / 2) * directionY;
            Vector2 pos = new Vector2(initPosX, initPosY);

            // New MatrixElementModelList
            MatrixElementModelList = new MatrixElementModel[MatrixSize.x, MatrixSize.y];

            for (int i = 0; i < MatrixSize.x; ++i)
            {
                for (int j = 0; j < MatrixSize.y; ++j)
                {
                    // View
                    MatrixElementView elemntView = Instantiate(MatrixElement, MatrixParent);
                    RectTransform trans = elemntView.GetComponent<RectTransform>();
                    trans.anchoredPosition = pos;
                    pos.x += directionX * (sizeElement + spacingX);

                    // Model
                    int posInMatrix = GetPosInMatrix(i, j);
                    MatrixElementType elementType = DataManager.Instance.MatrixLevelData.MatrixLevelList[0].MatrixList[0].ElementList[posInMatrix].Type;
                    MatrixElementModel elementModel = new MatrixElementModel();

                    elementModel.Init(elemntView, elementType, i, j);
                    MatrixElementModelList[i, j] = elementModel;

                }
                pos.y += directionY * (sizeElement + spacingY);
                pos.x = initPosX;
            }
        }

        public void FlowDown(int y)
        {
            StartCoroutine(DetailFlowDown(y));
        }

        private IEnumerator DetailFlowDown(int y)
        {
            yield return new WaitForSeconds(0.5f);
            int distance = 0;
            int start_space = -1;
            int end_space = -1;
            for (int i = MatrixSize.x - 1; i >= 0; --i)
            {
                if (this.MatrixElementModelList[i, y].IsMatch)
                {
                    if (end_space == -1 && start_space == -1)
                    {
                        end_space = i;
                    }
                    distance++;
                }
                else if (end_space != -1)
                {
                    start_space = i + 1;
                    break;
                }
            }
            int v = start_space;
            if (start_space > 0 && distance > 0)
            {
                for (int i = 0; i < start_space; ++i)
                {
                    v--;
                    if (v < 0) break;
                    if (!this.MatrixElementModelList[v, y].IsMatch) 
                    {
                        Vector2 pos = new Vector2(-1, -1);
                        pos = this.MatrixElementModelList[v + distance, y].MatrixElementView.GetComponent<RectTransform>().anchoredPosition;
                        StartCoroutine(SwapMatrixElementModel(new Vector2Int(v, y), new Vector2Int(v + distance, y), pos));
                    }
                }
            }
        }

        public int GetPosInMatrix(int i, int j)
        {
            if (i >= 0 && i < DataManager.Instance.MatrixData.InformationMatrix.Size.x &&
                j >= 0 && j < DataManager.Instance.MatrixData.InformationMatrix.Size.y)
            {
                return i * DataManager.Instance.MatrixData.InformationMatrix.Size.y + j;
            }
            return -1;
        }

        private Direction GetDirection()
        {
            Direction dir = Direction.None;
            if (Input.touchCount > 0)
            {
                Touch theTouch = Input.GetTouch(0);

                if (theTouch.phase == TouchPhase.Began)
                {
                    startPos = theTouch.position;
                }
                else if (theTouch.phase == TouchPhase.Ended)
                {
                    endPos = theTouch.position;

                    float x = endPos.x - startPos.x;
                    float y = endPos.y - startPos.y;

                    if (Mathf.Abs(x) <= 0.1 && Mathf.Abs(y) <= 0.1)
                    {
                        dir = Direction.None;
                    }
                    else if (Mathf.Abs(x) > Mathf.Abs(y))
                    {
                        dir = x > 0 ? Direction.Right : Direction.Left;
                    }
                    else
                    {
                        dir = y > 0 ? Direction.Up : Direction.Down;
                    }
                }
            }
            return dir;
        }
    }
}