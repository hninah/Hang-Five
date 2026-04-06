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

    void particle(string text, Vector3 position)
    {
        position = sceneCamera.WorldToScreenPoint(Player.Instance.transform.position + baseOffset);
        position.x += Random.Range(-offsetX, offsetX);

        GameObject particle = Instantiate(textParticle, position, Quaternion.identity, canvasTransform) as GameObject;
        particle.GetComponent<TextParticle>().Text = text;
    }

    public void generateTrickParticle()
    {
        if (!CanSpawnParticles()) return;

        particle(trickPhrases[Random.Range(0, trickPhrases.Count)], Player.Instance.transform.position + baseOffset);
    }

    public void generateNearMissParticle()
    {
        if (!CanSpawnParticles()) return;

        particle(nearMissPhrase, Player.Instance.transform.position + baseOffset);
    }

    public void generateScoreParticle(int amount)
    {
        if (!CanSpawnParticles()) return;

        particle("+" + amount.ToString(), Player.Instance.transform.position + baseOffset);
    }
    bool CanSpawnParticles()
    {
        // should not be able to spawn particles while in the death screen
        if (Player.Instance == null) return false;

        var state = Player.Instance.State;

        return state == Player.PlayerState.SURFING ||
               state == Player.PlayerState.FLIPPING;
    }
}
