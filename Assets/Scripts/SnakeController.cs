using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class SnakeController : MonoBehaviour {

    // Settings
    public float MoveSpeed = 15;
    public float SteerSpeed = 120;
    public float BodySpeed = 15;
    public int Gap = 20;
    public Text MyText;
    public Text Score;
    public GameObject Panel;
    private int score = 0;
    private Vector3 small;
    private Vector3 normal;
    public AudioSource EatingSound;
    public AudioSource CrashSound;
    
    // References
    public GameObject BodyPrefab;
    public GameObject FoodPrefab;

    // Lists
    private List<GameObject> BodyParts = new List<GameObject>();
    private List<Vector3> PositionsHistory = new List<Vector3>();

    private void Awake()
    {
        Panel.SetActive(false);
    }
    void Start() {
        GetComponent<Collider>().enabled = false;
        //GrowSnake();
        StartCoroutine(EnableCollision(0.3f));
        MyText.text = "Score: " + score;
        // создание первого хвостика
        GameObject body = Instantiate(BodyPrefab);
        BodyParts.Add(body);
        normal = body.transform.localScale;
        small = new Vector3(normal.x/2, normal.y/2, normal.z/2);
        body.transform.localScale = small;
        Vector3 randomPosition = new Vector3(Random.Range(-47f, 49f), 4.82f, Random.Range(-44f, 47f));
        Instantiate(FoodPrefab, randomPosition, Quaternion.identity);
    }

    
    void Update() {

        // Движение вперед
        transform.position += transform.forward * MoveSpeed * Time.deltaTime;

        // Поворот
        float steerDirection = Input.GetAxis("Horizontal");
        transform.Rotate(Vector3.up * steerDirection * SteerSpeed * Time.deltaTime);

        // Сохранение истории перемещений
        PositionsHistory.Insert(0, transform.position);

        // Перемещение тела
        int index = 1;
        foreach (var body in BodyParts) {
            Vector3 point = PositionsHistory[Mathf.Clamp(index * Gap, 0, PositionsHistory.Count - 1)];

            // Движение тела
            Vector3 moveDirection = point - body.transform.position;
            body.transform.position += moveDirection * BodySpeed * Time.deltaTime;

            // Поворот тела
            body.transform.LookAt(point);

            index++;
        }
    }

    //Реакция на столкновения
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Food")
        {
            EatingSound.Play();
            score += 10;
            MyText.text = "Score: " + score;
            Destroy(collision.gameObject);
            GetComponent<Collider>().enabled = false;
            GrowSnake();
            StartCoroutine(EnableCollision(0.3f));
            float x = Random.Range(-47f, 49f);
            float y = 4.82f;
            float z = Random.Range(-44f, 47f);
            Vector3 randomPosition = new Vector3(x, y, z);
            Instantiate(FoodPrefab, randomPosition, Quaternion.identity);
        }

        if (collision.gameObject.tag == "Wall" || collision.gameObject.tag == "Body")
        {
            CrashSound.Play();
            Score.text = MyText.text;
            Time.timeScale = 0;
            Panel.SetActive(true);

           // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex,LoadSceneMode.Single);
        }
    }

    //Рост змеи
    private void GrowSnake() {
        BodyParts.Last().transform.localScale = normal;
        //позиция последнего хвостика в листе BodyParts     
        GameObject body = Instantiate(BodyPrefab, BodyParts.Last().transform.position, Quaternion.identity);
        
        BodyParts.Add(body);
        BodyParts.Last().transform.localScale = small;

    }

    //Таймер включения коллизий
    private IEnumerator EnableCollision(float delay)
    {
        yield return new WaitForSeconds(delay);
        GetComponent<Collider>().enabled = true;
    }
   public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
        Time.timeScale = 1;
    }
}