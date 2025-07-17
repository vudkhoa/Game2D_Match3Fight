///
/// Create by linh soi - Abi Game studio
/// mentor Minh tito - CTO Abi Game studio
/// 
/// Manage list UI canvas for easy to use
/// Member nen inherit UI canvas
/// 
/// Update: 09-10-2020 
///             manage UI with Generic
///         09-10-2021 
///             Open, Close UI with Typeof(T)
///         28/11/2022
///             Close All UI
///             Close delay time
///

namespace UI
{
    using CustomUtils;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Events;

    public class UIManager : SingletonMono<UIManager>
    {
        //dict for quick query UI prefab
        //dict dung de lu thong tin prefab canvas truy cap cho nhanh
        private Dictionary<System.Type, UICanvas> _uiCanvasPrefab = 
            new Dictionary<System.Type, UICanvas>();

        //list from resource
        //list load ui resource
        private UICanvas[] _uiResources;

        //dict for UI active
        //dict luu cac ui dang dung
        private Dictionary<System.Type, UICanvas> _uiCanvas = 
            new Dictionary<System.Type, UICanvas>();

        //canvas container, it should be a canvas - root
        //canvas chua dung cac canvas con, nen la mot canvas - root de chua cac canvas nay
        public Transform CanvasParentTF;

        #region Canvas

        //open UI
        //mo UI canvas
        public T OpenUI<T>() where T : UICanvas
        {
            UICanvas canvas = GetUI<T>();

            canvas.Setup();
            canvas.Open();

            return canvas as T;
        }

        //check UI is loaded
        //kiem tra UI da duoc khoi tao hay chua
        public bool IsLoaded<T>() where T : UICanvas
        {
            System.Type type = typeof(T);
            return this._uiCanvas.ContainsKey(type) && this._uiCanvas[type] != null;
        }

        //Get component UI 
        //lay component cua UI hien tai
        public T GetUI<T>() where T : UICanvas
        {
            if (!this.IsLoaded<T>())
            {
                UICanvas canvas = Instantiate(GetUIPrefab<T>(), CanvasParentTF);
                this._uiCanvas[typeof(T)] = canvas;
            }

            return this._uiCanvas[typeof(T)] as T;
        }

        //Get prefab from resource
        //lay prefab tu Resources/UI
        private T GetUIPrefab<T>() where T : UICanvas
        {
            if (!this._uiCanvasPrefab.ContainsKey(typeof(T)))
            {
                if (this._uiResources == null) 
                {
                    this._uiResources = Resources.LoadAll<UICanvas>("UI");
                }

                Debug.Log(this._uiResources);

                for (int i = 0; i < this._uiResources.Length; i++)
                {
                    if (this._uiResources[i] is T)
                    {
                        this._uiCanvasPrefab[typeof(T)] = this._uiResources[i];
                        break;
                    }
                }
            }

            return this._uiCanvasPrefab[typeof(T)] as T;
        }


        public void CloseUI<T>() where T : UICanvas
        {
            if (IsOpened<T>())
            {
                GetUI<T>().CloseDirectly(); 
            }
        }

        //close UI with delay time
        //dong ui canvas sau delay time
        public void CloseUI<T>(float delayTime) where T : UICanvas
        {
            if (IsOpened<T>())
            {
                GetUI<T>().Close(delayTime);
            }
        }

        //Close all UI
        //dong tat ca UI ngay lap tuc -> tranh truong hop dang mo UI nao dong ma bi chen 2 UI cung mot luc
        public void CloseAll()
        {
            foreach (var item in this._uiCanvas)
            {
                if (item.Value != null && item.Value.gameObject.activeInHierarchy)
                {
                    item.Value.CloseDirectly();
                }
            }
        }

        //check UI is Opened
        //kiem tra UI dang duoc mo len hay khong
        public bool IsOpened<T>() where T : UICanvas
        {
            return this.IsLoaded<T>() && this._uiCanvas[typeof(T)].gameObject.activeInHierarchy;
        }

        #endregion

        #region Back Button
        private Dictionary<UICanvas, UnityAction> BackActionEvents =
             new Dictionary<UICanvas, UnityAction>();
        private List<UICanvas> _backCanvas = new List<UICanvas>();

        UICanvas BackTopUI
        {
            get
            {
                UICanvas canvas = null;
                if (this._backCanvas.Count > 0)
                {
                    canvas = this._backCanvas[this._backCanvas.Count - 1];
                }

                return canvas;
            }
        }

        public void PushBackAction(UICanvas canvas, UnityAction action)
        {
            if (!this.BackActionEvents.ContainsKey(canvas))
            {
                this.BackActionEvents.Add(canvas, action);
            }
        }
        
        public void AddBackUI(UICanvas canvas)
        {
            if (!this._backCanvas.Contains(canvas))
            {
                this._backCanvas.Add(canvas);
            }
        }

        public void RemoveBackCanvas(UICanvas canvas)
        {
            this._backCanvas.Remove(canvas);
        }

        #endregion
    }
}
