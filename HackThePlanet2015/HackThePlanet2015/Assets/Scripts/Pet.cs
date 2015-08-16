using System.Runtime.Remoting;
using UnityEngine;

namespace Tamagotchi
{
    public class Pet : MonoBehaviour
    {
        #region Variables
        [Range(0.0f, 100.0f)]
        public float health = 100f;
        [SerializeField]
        private HealthStage _currentHealthStage = HealthStage.FINE;
        [Range(0.0f, 100.0f)]
        public float hunger = 100f;
        [SerializeField]
        private HungerStage _currentHungerStage = HungerStage.BLOATED;
        [Range(0.0f, 100.0f)]
        public float happiness = 100f;
        [SerializeField]
        private HappinessStage _currentHappinessStage = HappinessStage.JOYFUL;

        
        [SerializeField]
        private AIStates _currentAIState = AIStates.IDLING;

        public static Pet Instance;

        private const float BASE_HUNGER_RATE = 0.5f;
        public int numShitAroundYou = 0;

        public float consumeRadius = 0.2f;
        public float annoyanceRadius = 0.8f;

        public float stablizeCooldown = 3.0f;
        public float stablizeTime = 0.0f;

        public void ModifyHealth(float num)
        {
            health += num;
        }

        public void ModifyHunger(float num)
        {
            hunger += num;
        }

        public void ModifyHappiness(float num)
        {
            happiness += num;
        }

        public bool CanPlay()
        {
            if (_currentHealthStage == HealthStage.SICK ||
                _currentHealthStage == HealthStage.DEAD_LIKE_MY_HEART ||
                _currentHungerStage == HungerStage.STARVING)
                return false;

            return true;
        }

        public bool IsAlive()
        {
            return _currentHealthStage != HealthStage.DEAD_LIKE_MY_HEART;
        }
        #endregion

        void OnEnable()
        {
            Instance = this;
        }

        void OnDisable()
        {
            Instance = null;
        }
        void Update()
        {
            CheckHealth();
            CheckHunger();
            CheckHappiness();

            if (IsAlive())
            {
                LowerHungerValue();
                LowerHealthValue();
                LowerHappinessValue();

                CheckForConsumablesNearby();
                CheckForAnnoyance();

                Stabalize();
                LookAtPlayer();
                
            }
        }

        #region AI Behavior
        void Stabalize()
        {
            if (Time.time > stablizeTime + stablizeCooldown)
            {
                stablizeTime = Time.time;
                GetComponent<Rigidbody>().velocity = Vector3.zero;
                GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
              
            }

        }
        void LookAtPlayer()
        {
            var targetPoint = Player.Instance.transform.position;
            var targetRotation = Quaternion.LookRotation(targetPoint - transform.position);
            // Smoothly rotate towards the target point.
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 3.4f * Time.deltaTime);   
        
        }
        /// <summary>
        /// The Pet will not eat food when sick.
        /// </summary>
        void Eat(IConsumable consumable)
        {
            if (_currentHealthStage != HealthStage.SICK ||
               IsAlive())
            {
                consumable.Consume();
            }
            else if (IsAlive())
            {
                if (consumable is Medicine)
                    consumable.Consume();
            }
        }
        #endregion

        #region Check States
        void CheckHealth()
        {
            if (health > 40f)
                _currentHealthStage = HealthStage.FINE;
            else if (health > 1f)
                _currentHealthStage = HealthStage.SICK;
            else
                _currentHealthStage = HealthStage.DEAD_LIKE_MY_HEART;
        }
        void CheckHunger()
        {
            if (hunger > 90f)
                _currentHungerStage = HungerStage.BLOATED;
            else if (hunger > 30f)
                _currentHungerStage = HungerStage.SATISFIED;
            else
                _currentHungerStage = HungerStage.STARVING;
                    
        }
        void CheckHappiness()
        {
            if (happiness > 80f)
                _currentHappinessStage = HappinessStage.JOYFUL;
            else if (happiness > 40f)
                _currentHappinessStage = HappinessStage.CONTENT;
            else 
                _currentHappinessStage = HappinessStage.MISERABLE;
        }

        void CheckAI()
        {
            
        }
        #endregion

        #region StatDepletion
        void LowerHungerValue()
        {
            hunger -= Time.deltaTime * HungerRate();
        }
        void LowerHappinessValue()
        {
            happiness -= Time.deltaTime * UnhappinessRate();
        }
        void LowerHealthValue()
        {
            health -= Time.deltaTime * UnhealthyRate();
        }
        /// <summary>
        /// The rate in which hunger decreases is determined by a constant base rate. Not yet sure whether other states affect this rate.
        /// </summary>
        float HungerRate()
        {
            float rate = 0;

            return BASE_HUNGER_RATE + rate;
        }
        /// <summary>
        /// The rate in which the happiness decreases is determined by whether or not the pet is hungry or is currently sick.
        /// </summary>
        float UnhappinessRate()
        {
            float rate = 0;

            if (_currentHungerStage == HungerStage.STARVING)
                rate += 0.5f;
            if (_currentHealthStage == HealthStage.SICK)
                rate += 0.5f;

            return rate;
        }
        /// <summary>
        /// The rate in which health decreases is determined by whether or not the pet is hungry or is unhappy.
        /// </summary>
        float UnhealthyRate()
        {
            float rate = 0;

            if (_currentHungerStage == HungerStage.STARVING)
                rate += 0.5f;
            if (_currentHappinessStage == HappinessStage.MISERABLE)
                rate += 0.5f;

            rate += (numShitAroundYou/50.0f);

            return rate;
        }
        #endregion

        #region AOE Check
        void CheckForConsumablesNearby()
        {
            // Check if we pinched a movable object and grab the closest one that's not part of the hand.
            Collider[] close_things = Physics.OverlapSphere(transform.position, consumeRadius);
            Vector3 distance = new Vector3(consumeRadius, 0.0f, 0.0f);

            for (int j = 0; j < close_things.Length; ++j)
            {
                Vector3 new_distance = transform.position - close_things[j].transform.position;
                if (close_things[j].GetComponent<IConsumable>() != null && new_distance.magnitude < distance.magnitude &&
                    !close_things[j].transform.IsChildOf(transform))
                {
                    var consumable = close_things[j].GetComponent<IConsumable>();
                    Eat(consumable);
                    
                    distance = new_distance;
                }
            }
        }
        void CheckForAnnoyance()
        {
            Collider[] close_things = Physics.OverlapSphere(transform.position, annoyanceRadius);
            Vector3 distance = new Vector3(annoyanceRadius, 0.0f, 0.0f);

            numShitAroundYou = close_things.Length;
        }
      
        void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, consumeRadius);

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, annoyanceRadius);
        }
        #endregion
    }
    /// <summary>
    /// Decrease Health by eating Candy, being surrounded by things.
    /// Increase Health by taking Medicine and Exercising.
    /// </summary>
    public enum HealthStage
    {
        FINE,
        SICK,
        DEAD_LIKE_MY_HEART
    }
    /// <summary>
    /// Decrease Hunger by eating food.
    /// Increase Hunger by exercising?
    /// </summary>
    public enum HungerStage
    {
        BLOATED,
        SATISFIED,
        STARVING
    }
    /// <summary>
    /// Decrease Happiness by taking Medicine.
    /// Increase Happiness by eating Candy and Exercising.
    /// </summary>
    public enum HappinessStage
    {
        JOYFUL,
        CONTENT,
        MISERABLE
    }
    /// <summary>
    /// IDLING - The Pet bobs around a certain point.
    /// PLAYING - The Pet is currently engaged in a game with the Player.
    /// ROAMING - The Pet finds a random point within the player sphere and flies to it.
    /// RETURNING - The Pet was previously knocked out the player sphere and is returning to a random point in the player sphere.
    /// </summary>
    public enum AIStates
    {
        IDLING,
        PLAYING,
        ROAMING,
        RETURNING

    }


 

}