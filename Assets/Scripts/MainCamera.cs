using System.Linq;
using UnityEngine;

public class MainCamera : MonoBehaviour {

    public GameObject CameraMain;

    public int CamSpeed = 1;
    public int GuiSize = 80;

    //Variables for the zoom. CURRENTLY DISABLED
    //private float _zoomAmount = 0f;
    //private float _ROTSpeed = 55f;
    //Variables for the general cameraspeed and the speed of the camera at the end of a level
    private float _cameraSpeed = 2f;
    private float _endSpeed = 50f;

    void Awake() {
        
    }

    void Update() {
        if (MenuScript.LevelFinished) {
            MoveToWall();
        }

        //Zoom function. Need to find out how to reset if level is finished.
        //DISABLED CURRENTLY DUE TO A LACK OF RESET ON WIN/LOSE CONDITION
        //if(MenuScript.LevelFinished == false) {
        //    _zoomAmount += Input.GetAxis("Mouse ScrollWheel");
        //    _zoomAmount = Mathf.Clamp(_zoomAmount, -GameManager.CameraMaxToClamp, GameManager.CameraMaxToClamp);
        //    float translate = Mathf.Min(Mathf.Abs(Input.GetAxis("Mouse ScrollWheel")),
        //        GameManager.CameraMaxToClamp - Mathf.Abs(_zoomAmount));
        //    Camera.main.transform.Translate(0, 0, translate * _ROTSpeed * Mathf.Sign(Input.GetAxis("Mouse ScrollWheel")));
        //}

        //######This function lets you move the camera with the mouse. Not good if not in full screen ######\\
        //var recdown = new Rect(0, 0, Screen.width, GuiSize);
        //var recup = new Rect(0, Screen.height - GuiSize, Screen.width, GuiSize);
        //var recleft = new Rect(0, 0, GuiSize, Screen.height);
        //var recright = new Rect(Screen.width - GuiSize, 0, GuiSize, Screen.height);

        //if(recdown.Contains(Input.mousePosition)) {
        //    transform.Translate(0, 0, CamSpeed, Space.World);
        //}

        //if(recup.Contains(Input.mousePosition)) {
        //    transform.Translate(0, 0, -CamSpeed, Space.World);
        //}

        //if(recleft.Contains(Input.mousePosition)) {
        //    transform.Translate(CamSpeed, 0, 0, Space.World);
        //}

        //if(recright.Contains(Input.mousePosition)) {
        //    transform.Translate(-CamSpeed, 0, 0, Space.World);
        //}

        //This will make sure that the level isn't finished, and allows the Player to move the camera
        // either with the arrow keys or WASD
        if(MenuScript.LevelFinished == false) {
            InputStatements();
        }
    }

    //Function for movement of camera
    //NOTE: Must set a variable for speed and also see if the code can be more efficient
    public void InputStatements() {
        if(Input.GetKey("d") && gameObject.transform.position.x >= GameManager.CameraRightLimit ||
            Input.GetKey("right") && gameObject.transform.position.x >= GameManager.CameraRightLimit) {
            transform.Translate(Vector3.left * _cameraSpeed);
        }

        if(Input.GetKey("a") && gameObject.transform.position.x <= GameManager.CameraLeftLimit ||
            Input.GetKey("left") && gameObject.transform.position.x <= GameManager.CameraLeftLimit) {
            transform.Translate(-Vector3.left * _cameraSpeed);
        }

        if(Input.GetKey("w") && gameObject.transform.position.z >= GameManager.CameraUpperLimit ||
            Input.GetKey("up") && gameObject.transform.position.z >= GameManager.CameraUpperLimit) {
            transform.Translate(-Vector3.forward * _cameraSpeed);
        }

        if(Input.GetKey("s") && gameObject.transform.position.z <= GameManager.CameraLowerLimit ||
            Input.GetKey("down") && gameObject.transform.position.z <= GameManager.CameraLowerLimit) {
            transform.Translate(Vector3.forward * _cameraSpeed);
        }
    }

    //Set the step speed and move the camera towards either the EnemyWall or the PlayerWall accordingly
    public void MoveToWall() {
        float step = _endSpeed * Time.deltaTime;
        if(MenuScript.LevelWinner == "Enemy") {
            transform.position = Vector3.MoveTowards(transform.position, GameManager.FriendlyWallCamera, step);
        }

        if(MenuScript.LevelWinner == "Player") {
            transform.position = Vector3.MoveTowards(transform.position, GameManager.EnemyWallCamera, step);
        }
    }
}