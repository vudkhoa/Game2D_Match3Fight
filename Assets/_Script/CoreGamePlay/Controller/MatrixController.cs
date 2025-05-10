using CustomData;
using CustomUtils;
using UnityEngine;

public class MatrixController : SingletonMono<MatrixController>
{
    [Header("Matrix Draw")]
    public GameObject MatrixElement;
    public RectTransform MatrixParent;
    public MatrixData MatrixData;

    protected override void Awake()
    {
        this.CreateMatrix();
    }

    // Draw Matrix 6x6
    private void CreateMatrix()
    {
        int directionX = MatrixData.InformationMatrix.Direction.x;
        int directionY = MatrixData.InformationMatrix.Direction.y;
        float widthMatrix = MatrixData.InformationMatrix.Width;
        float heightMatrix = MatrixData.InformationMatrix.Height;
        float paddingX = MatrixData.InformationMatrix.Padding.x;
        float paddingY = MatrixData.InformationMatrix.Padding.y;
        float sizeElement = MatrixData.InformationElement.Size;
        float spacingX = MatrixData.InformationMatrix.Spacing.x;
        float spacingY = MatrixData.InformationMatrix.Spacing.y;

        float initPosX = -directionX * (widthMatrix / 2) +
                         paddingX * directionX + (sizeElement / 2) * directionX;

        float initPosY = -directionY * (heightMatrix / 2) +
                         paddingY * directionY + (sizeElement / 2) * directionY;

        Vector2 pos = new Vector2(initPosX, initPosY);

        for (int i = 0; i < MatrixData.InformationMatrix.Size.x; ++i)
        {
            for (int j = 0; j < MatrixData.InformationMatrix.Size.y; ++j)
            {
                GameObject matrixElementGO = Instantiate(MatrixElement, MatrixParent);
                RectTransform trans = matrixElementGO.GetComponent<RectTransform>();
                trans.anchoredPosition = pos;
                pos.x += directionX * (sizeElement + spacingX);
            }
            pos.y += directionY * (sizeElement + spacingY);
            pos.x = initPosX;
        }

    }
}
