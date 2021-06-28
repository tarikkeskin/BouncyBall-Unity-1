using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace cyclone
{
    public class Test_Particle : MonoBehaviour
    {

        public GameObject pistolParticlePrefab;
        public AmmoRound[] ammo = new AmmoRound[ammoRounds];
        GameObject []particle_G=new GameObject[ammoRounds];
        const int ammoRounds = 10;
        ShotType currentShotType=ShotType.PISTOL;

        public enum ShotType
        {
            UNUSED = 0,
            PISTOL,
            ARTILLERY,
            FIREBALL,
            LASER
        };
  
        [System.Serializable]
        public class AmmoRound
        {
            public cyclone.Particle particle;
            public cyclone.Test_Particle.ShotType type = ShotType.UNUSED;
            public float startTime;

            public AmmoRound(Particle particle, ShotType type, float startTime)
            {
                this.particle = particle;
                this.type = type;
                this.startTime = startTime;
            }
        }

        private void Start()
        {

            for (int i = 0; i < ammoRounds; i++)
            {
                AmmoRound round = new AmmoRound(new Particle(), ShotType.UNUSED, 0f);
                ammo[i] = round;
            }

        }

        void Fire()
        {
            AmmoRound shot;
            //Find the first available round.
            for (int i = 0; ; i++)
            {
                shot = ammo[i];
                if (ammo[i].type == ShotType.UNUSED)
                {
                    particle_G[i] = Instantiate(pistolParticlePrefab);
                    break;
                }
                if (i == ammoRounds-1) return;
            }
            
            // Set the properties of the particle
            switch (currentShotType)
            {
                case ShotType.PISTOL:
                    shot.particle.SetMass(2.0f); // 2.0kg
                    shot.particle.SetVelocity(0.0f, 0.0f, 35.0f); // 35m/s
                    shot.particle.SetAcceleration(0.0f, -1.0f, 0.0f);
                    shot.particle.SetDamping(0.99f);
                    break;

                case ShotType.ARTILLERY:
                    shot.particle.SetMass(200.0f); // 200.0kg
                    shot.particle.SetVelocity(0.0f, 30.0f, 40.0f); // 50m/s
                    shot.particle.SetAcceleration(0.0f, -20.0f, 0.0f);
                    shot.particle.SetDamping(0.99f);
                    break;

                case ShotType.FIREBALL:
                    shot.particle.SetMass(1.0f); // 1.0kg - mostly blast damage
                    shot.particle.SetVelocity(0.0f, 0.0f, 10.0f); // 5m/s
                    shot.particle.SetAcceleration(0.0f, 0.6f, 0.0f); // Floats up
                    shot.particle.SetDamping(0.9f);
                    break;

                case ShotType.LASER:
                    shot.particle.SetMass(0.1f); // 0.1kg - almost no weight
                    shot.particle.SetVelocity(0.0f, 0.0f, 100.0f); // 100m/s
                    shot.particle.SetAcceleration(0.0f, 0.0f, 0.0f); // No gravity
                    shot.particle.SetDamping(0.99f);
                    break;

            }

            // Set the data common to all particle types
            shot.particle.SetPosition(0.0f, 1.5f, 0.0f);
            shot.startTime = Time.time;
            shot.type = currentShotType;

            // Clear the force accumulators
            shot.particle.ClearAccumulator();

        }

        //update method for particles
        void ParticleUpdate()
        {
            float duration = Time.fixedDeltaTime;
            if (duration <= 0.0f) return;
            // Update the physics of each particle in turn
            AmmoRound shot;
            for (int i = 0; i < ammoRounds; i++)
            {
                shot = ammo[i];
                if (shot.type != ShotType.UNUSED)
                {
                    // Run the physics
                    shot.particle.Integrate(duration);
                    // Check if the particle is now invalid
                    if (shot.particle.GetPosition().y < 0.0f ||shot.startTime + 5000 < Time.time || shot.particle.GetPosition().z > 200.0f)
                    {
                        shot.type = ShotType.UNUSED;
                    }
                    else
                    {
                        particle_G[i].transform.position = new Vector3((float)shot.particle.position.x, (float)shot.particle.position.y, (float)shot.particle.position.z);
                    }
                }
            }
        }

        private void Update()
        {
            //get inputs if particle demo is active            

            if (Input.GetKeyDown("1"))
            {
                currentShotType = ShotType.PISTOL;
                Debug.Log("currentShotType->" + currentShotType);
            }
            if (Input.GetKeyDown("2"))
            {
                currentShotType = ShotType.ARTILLERY;
                Debug.Log("currentShotType->" + currentShotType);
            }
            if (Input.GetKeyDown("3"))
            {
                currentShotType = ShotType.FIREBALL;
                Debug.Log("currentShotType->" + currentShotType);
            }
            if (Input.GetKeyDown("4"))
            {
                currentShotType = ShotType.LASER;
                Debug.Log("currentShotType->" + currentShotType);
            }
            if (Input.GetButtonDown("Fire1"))
            {
                Fire();
            }

        }
        private void FixedUpdate()
        {
            ParticleUpdate();
        }
        
    };
}