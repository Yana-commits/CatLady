using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [Header("Scene")]
    [SerializeField] private EnemiesController enemies;
    [SerializeField] private CatController catController;
    [SerializeField] private LadyController ladyController;
    [SerializeField] private List<ParticleSystem> particles = new List<ParticleSystem>();
    void Awake()
    {
        enemies.OnLookForCat += LookforCat;
        catController.Initialise();
        enemies.Initialise();
        ladyController.OnHit += ShowSmile;
        catController.NotLookForCat += DontSeekCat;
        catController.OnGameOver += GameOver;
    }
    private void Start()
    {
      
        ladyController.Enemies = enemies.creatures;
    }

    public void LookforCat()
    {
        if(catController.FindFreeCat() != null)
        enemies.LookingCat(catController.FindFreeCat().transform);
    }
    public void ShowSmile(int id)
    {
        enemies.ShowEnemySmile(id);
    }

    public void DontSeekCat(int id)
    {
        enemies.StopLookingCat(id);
    }

    public void GameOver()
    {
        foreach (var party in particles)
        {
            party.gameObject.SetActive(true);
            party.Play();
        }
        ladyController.GameOver();

        StartCoroutine(Over()); 
    }
    private IEnumerator Over()
    {
        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    private void OnDisable()
    {
        enemies.OnLookForCat -= LookforCat;
        ladyController.OnHit -= ShowSmile;
        catController.NotLookForCat -= DontSeekCat;
        catController.OnGameOver -= GameOver;
    }
}
