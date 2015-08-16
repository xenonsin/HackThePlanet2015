
using System;
using UnityEngine;
using Random = UnityEngine.Random;

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
        [SerializeField]
        private IdleState _currentIdleState = IdleState.NONE;
        [SerializeField]
        private ReturningState _currentReturningState = ReturningState.NONE;
        [SerializeField]
        private RoamingState _currentRoamingState = RoamingState.NONE;

        public static Pet Instance;

        private const float BASE_HUNGER_RATE = 0.5f;
        public int numShitAroundYou = 0;

        public float consumeRadius = 0.2f;
        public float annoyanceRadius = 0.8f;

        public float stablizeCooldown = 3.0f;
        private float stablizeTime = 0.0f;

        public float floatingStrength = 100.0f;
        public float idleTimeLeft = 0.0f;


        public Vector3 targetLocation = new Vector3();
        private Vector3 lastLocation = new Vector3();

        private Rigidbody rb;

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

        public bool IsHealthy()
        {
            if (_currentHealthStage == HealthStage.SICK ||
                _currentHealthStage == HealthStage.DEAD_LIKE_MY_HEART)
                return false;

            return true;
        }

        public void Play()
        {
            _currentAIState = AIStates.PLAYING;
        }
        #endregion
        #region Unity STuff
        void OnEnable()
        {
            Instance = this;
        }

        void OnDisable()
        {
            Instance = null;
        }

        void Awake()
        {
            rb = GetComponent<Rigidbody>();
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

                CheckParalyze();
                ActBaseOnAIState();

            }
        }
        #endregion
        #region AI Behavior


        void ActBaseOnAIState()
        {
            switch (_currentAIState)
            {
                case AIStates.PARALYZED:
                    break;
                case AIStates.DECIDING:
                    Deciding();
                    break;
                case AIStates.IDLING:
                    Idle();
                    break;
                case AIStates.PLAYING:
                    break;
                case AIStates.ROAMING:
                    Roaming();
                    break;
                case AIStates.RETURNING:
                    Returning();
                    break;
                default:
                    break;
            }
        }

        void Deciding()
        {
            ResetAIStates();

            if (IsOutOfPlayerLimits())
            {
                _currentAIState = AIStates.RETURNING;
                if (_currentReturningState == ReturningState.NONE)
                    _currentReturningState = ReturningState.LOOKING_FOR_TARGET;
            }
            else
            {
                 _currentAIState = Random.value < 0.6f ? AIStates.IDLING : AIStates.ROAMING;
            }

           
        }

        //Pet bobs around a point.
        void Idle()
        {
            switch (_currentIdleState)
            {
                case IdleState.NONE:
                    break;
                case IdleState.RECORDING_POSITON:
                    lastLocation = transform.position;
                    idleTimeLeft = Random.Range(4, 10);
                    _currentIdleState = IdleState.DOIN_MY_THANG;
                    break;
                case IdleState.DOIN_MY_THANG:
                    transform.position = new Vector3(transform.position.x,lastLocation.y + ((float)Math.Sin(Time.time) / floatingStrength),transform.position.z);
                    idleTimeLeft -= Time.deltaTime;
                    if (idleTimeLeft < 0f)
                        _currentAIState = AIStates.DECIDING;
                    break;
            }
            //Currently Only Moves Up and Down.
           
        }

        void Roaming()
        {
            switch (_currentRoamingState)
            {
                case RoamingState.NONE:
                    break;
                case RoamingState.LOOKING_FOR_TARGET:
                    targetLocation = RandomPointInPlayerSphere();
                    _currentRoamingState = RoamingState.MOVING;
                    break;
                case RoamingState.MOVING:
                    transform.position = Vector3.MoveTowards(transform.position, targetLocation,Time.deltaTime / 4);
                    if (ReachedTarget())
                        _currentRoamingState = RoamingState.REACHED_TARGET;
                    break;
                case RoamingState.REACHED_TARGET:
                    _currentAIState = AIStates.DECIDING;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        void Returning()
        {
            switch (_currentReturningState)
            {
                case ReturningState.NONE:
                    break;
                case ReturningState.LOOKING_FOR_TARGET:
                    targetLocation = RandomPointInPlayerSphere();
                    _currentReturningState = ReturningState.MOVING;

                    break;
                case ReturningState.MOVING:
                    transform.position = Vector3.MoveTowards(transform.position, targetLocation,Time.deltaTime / 4);
                    if (ReachedTarget())
                        _currentReturningState = ReturningState.REACHED_TARGET;
                    break;
                case ReturningState.REACHED_TARGET:
                    _currentAIState = AIStates.DECIDING;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        void Stabalize()
        {
            
            if (Time.time > stablizeTime + stablizeCooldown && _currentAIState == AIStates.PARALYZED)
            {
                stablizeTime = Time.time;
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
              
                _currentAIState = AIStates.DECIDING;
                
            }

        }

        void ResetAIStates()
        {
            _currentReturningState = ReturningState.NONE;
            _currentIdleState = IdleState.NONE;
        }
        void LookAtPlayer()
        {
            var targetPoint = Player.Instance.transform.position;
            var targetRotation = Quaternion.LookRotation(targetPoint - transform.position);
            // Smoothly rotate towards the target point.
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 2 * Time.deltaTime);   
        
        }
        /// <summary>
        /// The Pet will not eat food when sick.
        /// </summary>
        void Eat(IConsumable consumable)
        {
            if (IsHealthy())
            {
                consumable.Consume();
            }
            else if (IsAlive())
            {
                if (consumable is Medicine)
                    consumable.Consume();
            }
        }

        Vector3 RandomPointInPlayerSphere()
        {
            var position = Player.Instance.transform.position + Random.onUnitSphere / 2;
            if (position.y < 0)
                position.y = 1;
            return position;
        }

        bool ReachedTarget()
        {
            return Vector3.Distance(transform.position, targetLocation) < 0.01f;
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
            {
                health = 0;
                _currentHealthStage = HealthStage.DEAD_LIKE_MY_HEART;
            }
        }
        void CheckHunger()
        {
            if (hunger > 90f)
                _currentHungerStage = HungerStage.BLOATED;
            else if (hunger > 30f)
                _currentHungerStage = HungerStage.SATISFIED;
            else if (hunger > 1f)
                _currentHungerStage = HungerStage.STARVING;
            else
                hunger = 0;
                    
        }
        void CheckHappiness()
        {
            if (happiness > 80f)
                _currentHappinessStage = HappinessStage.JOYFUL;
            else if (happiness > 40f)
                _currentHappinessStage = HappinessStage.CONTENT;
            else if (happiness > 1f)
                _currentHappinessStage = HappinessStage.MISERABLE;
            else
                happiness = 0;
        }

        void CheckParalyze()
        {
            if (rb.velocity != Vector3.zero && rb.angularVelocity != Vector3.zero)
                _currentAIState = AIStates.PARALYZED;
            


            //_currentAIState = AIStates.IDLING;
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

        bool IsOutOfPlayerLimits()
        {
            return Vector3.Distance(transform.position, Player.Instance.transform.position) > 0.6f;
        }

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
    /// PARALYZED - Pet cannot act, usually result of being hit.
    /// IDLING - The Pet bobs around a certain point.
    /// PLAYING - The Pet is currently engaged in a game with the Player.
    /// ROAMING - The Pet finds a random point within the player sphere and flies to it.
    /// RETURNING - The Pet was previously knocked out the player sphere and is returning to a random point in the player sphere.
    /// </summary>
    public enum AIStates
    {
        PARALYZED,
        DECIDING,
        IDLING,
        PLAYING,
        ROAMING,
        RETURNING

    }

    public enum ReturningState
    {
        NONE,
        LOOKING_FOR_TARGET,
        MOVING,
        REACHED_TARGET
    }

    public enum IdleState
    {
        NONE,
        RECORDING_POSITON,
        DOIN_MY_THANG

    }

    public enum RoamingState
    {
        NONE,
        LOOKING_FOR_TARGET,
        MOVING,
        REACHED_TARGET
    }


 

}