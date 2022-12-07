using UnityEngine;

public class ParticleMaster : MonoBehaviour
{
    //static here for ez cod
    //public static ParticleMaster partInstance;

    public GameObject entity { get; private set; }
    private ParticleSystem particle;

    /*
     public void OnValidate()
     {
         //make sure i don't crash unity with making sure i get the instantiate
         if (partInstance != null)
         {
             partInstance = this;
         }
     }
    */

    //for the time being, i will make this into an objectpool [ ]
    private ParticleSystem dustOnJump;
    private ParticleSystem dustOnImpact;
    private ParticleSystem dustOnMovement;
    private ParticleSystem healing;
    private ParticleSystem enemyDeath;



    public void Awake()
    {
        dustOnJump = Resources.Load<ParticleSystem>("Particles/dustOnJump");
        dustOnImpact = Resources.Load<ParticleSystem>("Particles/dustOnImpact");
        dustOnMovement = Resources.Load<ParticleSystem>("Particles/running");

        healing = Resources.Load<ParticleSystem>("Particles/healing");
        enemyDeath = Resources.Load<ParticleSystem>("Particles/EnemyDeathParticle");
    }
    public void setEntity(GameObject entityObject)
    {
        // May still be used but can be unnecesary
        entity = entityObject;
        Debug.Log(entityObject.name);
        return;
    }

    public void InstantiateParticle(string particleType, string particleName, Transform objectTransform)
    {
        if (particleType == "movement" && entity != null)
        {
            if (particleName == "dustOnJump") // you jump, dust below your feet
            {
                //loading the particle and printing it out to ensure it is the right one
                particle = dustOnJump;

                //instantiating the particle, playing, and destroying it after it's done playing. won't comment the next ones, but will split just like this so you know which is which

                ParticleSystem spawnParticle = Instantiate(particle, objectTransform.position, objectTransform.rotation, entity.transform);
                spawnParticle.Play();
                Destroy(spawnParticle.gameObject, spawnParticle.main.duration);
                //unload asset from memory

            }
            else if (particleName == "dustOnImpact") // you fall, dust on your feet when impact
            {
                particle = dustOnImpact;

                ParticleSystem spawnParticle = Instantiate(particle, objectTransform.position, objectTransform.rotation, entity.transform);
                spawnParticle.Play();
                Destroy(spawnParticle.gameObject, spawnParticle.main.duration);


            }
            else if (particleName == "dustOnMovement")// while running, dust on feet will happen.
            {
                particle = dustOnMovement;

                ParticleSystem spawnParticle = Instantiate(particle, objectTransform.position, Quaternion.identity, entity.transform);
                spawnParticle.Play();
                Destroy(spawnParticle.gameObject, spawnParticle.main.duration);

            }
            else
            {
                Debug.Log("Invalid arguments! Did you type something wrong? Check your capitalization, strings are stupid and can't tell if you did that wrong.");
            }
        }
        if (particleType == "combat" && entity != null)
        {
            if (particleName == "bleedOnHit") // bleed on damage
            {
                //add particle

                ParticleSystem spawnParticle = Instantiate(particle, objectTransform.position, Quaternion.identity, entity.transform);
                spawnParticle.Play();
                Destroy(spawnParticle.gameObject, spawnParticle.main.duration);

            }
            else if (particleName == "EnemyDeathParticle") // massive blood on death
            {
                //add particle

                ParticleSystem spawnParticle = Instantiate(particle, objectTransform.position, objectTransform.rotation, entity.transform);
                spawnParticle.Play();
                Destroy(spawnParticle.gameObject, 5f);

            }
            else if (particleName == "healing") // healing particles for healing potion
            {
                //add particle

                ParticleSystem spawnParticle = Instantiate(particle, objectTransform.position, objectTransform.rotation, entity.transform);
                spawnParticle.Play();
                Destroy(spawnParticle.gameObject, .5f);
            }
        }
    }
}
