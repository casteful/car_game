using UnityEngine;
using UnityEditor;

public class MenuItems
{
    [MenuItem("Tools/Create Car Game")]

    private static void MenuOption()
    {
        GameObject canvas = GameObject.Find("Canvas");
        if (canvas == null)
        {
            canvas = (GameObject)PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath<Object>("Assets/Prefabs/Canvas.prefab"));
        }

        GameObject gm = GameObject.Find("GameManager");
        if(gm == null)
        {
            gm = new GameObject("GameManager");
            gm.AddComponent<GameManager>();
        }

        GameObject road = GameObject.Find("road");
        if(road == null)
        {
            road = (GameObject)PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath<Object>("Assets/Prefabs/road.prefab"));
        }

        GameObject car = GameObject.Find("car");
        if (car == null)
        {
            car = (GameObject)PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath<Object>("Assets/Prefabs/car.prefab"));
            car.AddComponent<Rigidbody>();
            car.AddComponent<CarBehavior>();
        }

        GameObject camera = GameObject.Find("Main Camera");

        car.transform.position = new Vector3(0, 2, 0);

        if(camera.GetComponent<CameraController>() == null)
            camera.AddComponent<CameraController>();

        SetUpCamera(camera, car);

        SetUpCarBehavior(car.GetComponent<CarBehavior>());
        SetUpCarRigidbody(car.GetComponent<Rigidbody>());
    }

    private static void SetUpCarRigidbody(Rigidbody rb)
    {
        rb.mass = 500;
        rb.drag = 0;
        rb.angularDrag = 0.05f;
        rb.useGravity = true;
        rb.isKinematic = false;
        rb.interpolation = RigidbodyInterpolation.None;
        rb.collisionDetectionMode = CollisionDetectionMode.Discrete;
        rb.constraints = RigidbodyConstraints.None;
    }

    private static void SetUpCarBehavior(CarBehavior cb)
    {
        cb.carTorque = 70;
        cb.carAngle = 10;
        cb.brakeSpeed = 10000;
        cb.carDrive = 2;
        cb.naturalFrequency = 7;
        cb.dampingRatio = 0;
        cb.forceShift = 0.03f;
        cb.setSuspensionDistance = true;
    }

    private static void SetUpCamera(GameObject camera, GameObject car)
    {
        camera.transform.position = new Vector3(2, 4, -4);
        camera.transform.eulerAngles = new Vector3(35, -15, 0);
        camera.GetComponent<Camera>().clearFlags = CameraClearFlags.Color;
        camera.GetComponent<CameraController>().player = car;
        camera.GetComponent<CameraController>().offset = new Vector3(1, 2, -3);
    }
}