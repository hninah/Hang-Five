using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextParticleManager : MonoBehaviour
{
    private static TextParticleManager instance;
    public static TextParticleManager Instance { get { return instance; } }
    [SerializeField] List<string> trickPhrases = new List<string> { "Woah!", "Gnarly!", "Radical!", "Awesome!" };
    [SerializeField] string nearMissPhrase = "Close One!";
    [SerializeField] GameObject textParticle;
    [SerializeField] float offsetX = 0.3f;
    [SerializeField] Vector3 baseOffset = new Vector3(0.2f, 0.0f, 0.0f);
    [SerializeField] Transform canvasTransform;
    [SerializeField] Camera sceneCamera;
    List<TextParticle> textParticles = new List<TextParticle>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Awake()
    {
        if (Instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void LateUpdate()
    {
        if (textParticles.Count == 0) return;

        List<TextParticle> tempParticles = new List<TextParticle>();

        foreach (TextParticle particle in textParticles)
        {
            if (!particle)
            {
                continue;
            }

            tempParticles.Add(particle);
        }

        textParticles = tempParticles;
    }

    public void deactivateParticles()
    {
        foreach (TextParticle particle in textParticles)
        {
            if (!particle) continue;

            print("disabling particle");
            particle.gameObject.SetActive(false);
        }
    }

    public void activateParticles()
    {
        foreach (TextParticle particle in textParticles)
        {
            if (!particle) continue;

            particle.gameObject.SetActive(true);
        }
    }

    void particle(string text, Vector3 position)
    {
        position = sceneCamera.WorldToScreenPoint(Player.Instance.transform.position + baseOffset);
        position.x += Random.Range(-offsetX, offsetX);

        GameObject particle = Instantiate(textParticle, position, Quaternion.identity, canvasTransform) as GameObject;
        TextParticle textPart = particle.GetComponent<TextParticle>();
        textPart.Text = text;
        textParticles.Add(textPart);
    }

    public void generateTrickParticle()
    {
        particle(trickPhrases[Random.Range(0, trickPhrases.Count)], Player.Instance.transform.position + baseOffset);
    }

    public void generateNearMissParticle()
    {
        particle(nearMissPhrase, Player.Instance.transform.position + baseOffset);
    }

    public void generateScoreParticle(int amount)
    {
        particle("+" + amount.ToString(), Player.Instance.transform.position + baseOffset);
    }
}
