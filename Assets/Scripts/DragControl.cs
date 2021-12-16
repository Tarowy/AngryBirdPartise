using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragControl : MonoBehaviour
{
    //弹弓本身相关
    public Transform pivot;
    public float maxDistance;
    public bool isClicking;
    private Vector3 _shootDirection;
    private bool _canMove = false;
    private bool _isflying = false;
    //鸟的组件
    private GameObject _bird;
    private Rigidbody2D _rigidbody;
    private SpringJoint2D _springJoint2D;
    public AudioClip selectClip;
    public AudioClip flyClip;
    //画线的组件
    public LineRenderer leftLine;
    public Transform leftPos;
    public LineRenderer rightLine;
    public Transform rightPos;
    //摄像机跟随
    public Vector3 cameraPos;
    public float smooth = 3;

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale != 0)
        {
            if (!_isflying)
            {
                IfTouchSlingshot();
            }
        
            if (_canMove)
            {
                BirdsMoveWhenMouseClick();
                DrawLine();
            }
        }
        
        if (_bird != null)
        {
            CameraFollow();
        }
    }
    
    //初始化鸟的刚体、连接点
    public void InitComponet(GameObject bird)
    {
        _isflying = false;
        AudioSource.PlayClipAtPoint(selectClip,bird.transform.position);
        _bird = bird;
        _rigidbody = bird.GetComponent<Rigidbody2D>();
        _springJoint2D = bird.GetComponent<SpringJoint2D>();
        _springJoint2D.connectedBody = gameObject.GetComponent<Rigidbody2D>();
        _springJoint2D.connectedAnchor = new Vector2(pivot.localPosition.x,pivot.localPosition.y);
        _springJoint2D.enabled = true;
    }
    
    //取消弹簧的作用
    public virtual void BeingKinematic()
    {
        _springJoint2D.enabled = false;
        _rigidbody.gravityScale = 1;
    }
    
    /// <summary>
    /// 鸟上弹弓时使其跟随鼠标
    /// </summary>
    public void BirdsMoveWhenMouseClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _bird.GetComponent<Bird>().enabled = true;
            isClicking = true;
            _rigidbody.isKinematic = true;
            //需要让刚体休眠一下，否则发射的时候会出现偏差
            _rigidbody.Sleep();
            
            rightLine.enabled = true;
            leftLine.enabled = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            isClicking = false;
            _canMove = false;
            _rigidbody.isKinematic = false;
            ClearLine();
            AudioSource.PlayClipAtPoint(flyClip, transform.position);
            
            var circleCollider2Ds = _bird.GetComponents<CircleCollider2D>();
            foreach (var circleCollider2D in circleCollider2Ds)
            {
                circleCollider2D.enabled = true;
            }
            //让鸟在松手后的5秒后死亡
            _bird.GetComponent<Bird>().CountTimeToDie();
            _bird.GetComponent<TrailRenderer>().enabled = true;
            _bird.GetComponent<Bird>().isFlying = true;

            Invoke("BeingKinematic",0.1f);
        }
        
        if (isClicking)
        {
            _bird.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition)+new Vector3(0,0,10);
            //限定鼠标拖拽鸟的最大距离
            if (Vector3.Distance(_bird.transform.position,pivot.position) > maxDistance)
            {
                _shootDirection = (_bird.transform.position - pivot.position).normalized; //向量的方向由被减数指向减数
                _bird.transform.position = pivot.position + maxDistance*_shootDirection; //所限定的最大长度的矢量
            }
        }
    }
    
    public void DrawLine()
    {
        if (_springJoint2D.enabled)
        {
            rightLine.SetPosition(0,rightPos.position);
            rightLine.SetPosition(1,_bird.transform.position+_shootDirection*0.25f);
        
            leftLine.SetPosition(0,leftPos.position);
            leftLine.SetPosition(1,_bird.transform.position+_shootDirection*0.25f);
        }
    }

    public void ClearLine()
    {
        rightLine.enabled = false;
        leftLine.enabled = false;
    }
    
    public void CameraFollow()
    {
        //相机平滑跟随
        cameraPos = Camera.main.transform.position;
  
        Camera.main.transform.position = Vector3.Lerp(cameraPos,
            new Vector3(Mathf.Clamp(_bird.transform.position.x, 0, 15), cameraPos.y, cameraPos.z), smooth * Time.deltaTime);
    }


    /// <summary>
    /// 只有再弹簧锚点半径为1的区域内点击才能使鸟移动
    /// </summary>
    public void IfTouchSlingshot()
    {
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        var distance = Vector3.Distance(mousePos,pivot.position);
        
        //防止鸟飞出去之后又能激活移动状态
        if (distance <= 1 && Input.GetMouseButtonDown(0)) //场景刚开始加载的时候，鼠标处于的位置可能正好位于小于1的范围内，必须加上是否点击过的判定
        {
            _canMove = true;
            _isflying = true;
        }
    }
}
