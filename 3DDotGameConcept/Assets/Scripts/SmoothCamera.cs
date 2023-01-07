using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SmoothCamera : MonoBehaviour
{
    public AudioClip changeSound;
    public GameObject enemyLoader;
    public int leftOrTopEnemiesIndex;
    public int rightOrBottomEnemiesIndex;

    [SerializeField] private Camera room_camera;
    [SerializeField] private Transform player;
    [SerializeField] private Transform door;
    Vector3 dir;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player") return;
        dir = player.transform.position - door.transform.position;
        room_camera.gameObject.transform.position = room_camera.gameObject.transform.position;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag != "Player") return;
        Vector3 exit_dir = player.transform.position - door.transform.position;
        float orientation = door.gameObject.transform.rotation.eulerAngles.y;
        Vector3 movement = new Vector3(0.0f, 0.0f, 0.0f);

        if (orientation == 0.0f && dir.z > 0 && dir.z/exit_dir.z < 0) {
            movement = new Vector3(0.0f, 0.0f, -12.0f);
            enemyLoader.GetComponent<GameLoad>().LoadAndUnloadEnemies(rightOrBottomEnemiesIndex);
        }
        else if ((orientation == 0.0f && dir.z <= 0 && dir.z/exit_dir.z < 0) || (orientation == 180.0f)) {
            movement = new Vector3(0.0f, 0.0f, 12.0f);
            enemyLoader.GetComponent<GameLoad>().LoadAndUnloadEnemies(leftOrTopEnemiesIndex);
        }
        else if (orientation == 90.0f && dir.x > 0 && dir.x/exit_dir.x < 0) {
            movement = new Vector3(-12.0f, 0.0f, 0.0f);
            enemyLoader.GetComponent<GameLoad>().LoadAndUnloadEnemies(leftOrTopEnemiesIndex);
        }
        else if (orientation == 90.0f && dir.x <= 0 && dir.x/exit_dir.x < 0) {
            movement = new Vector3(12.0f, 0.0f, 0.0f);
            enemyLoader.GetComponent<GameLoad>().LoadAndUnloadEnemies(rightOrBottomEnemiesIndex);
        }

        if (orientation == 180.0f) gameObject.GetComponent<MeshCollider>().isTrigger = false;

        room_camera.gameObject.transform.position = room_camera.gameObject.transform.position + movement;

        if (changeSound)
				AudioSource.PlayClipAtPoint(changeSound, transform.position);
    }
}
