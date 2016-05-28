using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ParticleSystem))]
public class CFX_AutoDestructShuriken : MonoBehaviour
{
	public bool OnlyDeactivate;

    public float minPlay = 1.0f;
    public float maxPlay = 3.0f;
	
    void Awake()
    {
        particle = GetComponent<ParticleSystem>();
    }
	void OnEnable()
	{
		StartCoroutine("CheckIfAlive");
        StartEffect();
	}
	
	IEnumerator CheckIfAlive ()
	{
		while(true)
		{
			yield return new WaitForSeconds(0.5f);
			if(!GetComponent<ParticleSystem>().IsAlive(true))
			{
				if(OnlyDeactivate)
				{
					#if UNITY_3_5
						this.gameObject.SetActiveRecursively(false);
					#else
						this.gameObject.SetActive(false);
					#endif
				}
				else
					//GameObject.Destroy(this.gameObject);
				break;
			}
		}
	}

    private ParticleSystem particle;

    public void StartEffect()
    {
        StartCoroutine(Effect());
    }

    private IEnumerator Effect()
    {
        while(true)
        {
            if(!particle.isPlaying)
            {
                particle.Play();
            }
            yield return new WaitForSeconds(Random.Range(minPlay, maxPlay));
        }
    }
}
