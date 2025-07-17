namespace UI
{
    using UnityEngine;

    public class UICanvas : MonoBehaviour
    {
        public bool IsDestroyOnClose = false;

        protected RectTransform m_RectTransform;
        protected Animator m_Animator;

        private void Start()
        {
            this.OnInit();
        }

        //Init default Canvas
        //khoi tao gia tri canvas
        protected void OnInit()
        {
            this.m_RectTransform = this.GetComponent<RectTransform>();
            this.m_Animator = this.GetComponent<Animator>();
        }

        //Setup canvas to avoid flash UI
        //set up mac dinh cho UI de tranh truong hop bi nhay' hinh
        public virtual void Setup()
        {
            UIManager.Instance.AddBackUI(this);
            UIManager.Instance.PushBackAction(this, BackKey);
        }

        //back key in android device
        //back key danh cho android
        public virtual void BackKey()
        {

        }

        //Open canvas
        //mo canvas
        public virtual void Open()
        {
            this.gameObject.SetActive(true);
        }

        //close canvas directly
        //dong truc tiep, ngay lap tuc
        public virtual void CloseDirectly()
        {
            UIManager.Instance.RemoveBackCanvas(this);  
            gameObject.SetActive(false);
            if (this.IsDestroyOnClose)
            {
                Destroy(gameObject);
            }
        }

        public virtual void Close(float delayTime)
        {
            Invoke(nameof(CloseDirectly), delayTime);
        }
    }
}

