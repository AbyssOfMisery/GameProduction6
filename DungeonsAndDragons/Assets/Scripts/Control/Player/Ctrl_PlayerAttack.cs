﻿/*
 * Title:"Dungoen and dragons"
 *      
 *      Control layer: player attack
 *      
 * Description:
 *        1. put nearby enemies in a enemy array
 *          1.1 get all enemy and put them in an array
 *          1.2 check enemy array and find out the closest enemy
 *          
 *        2. player will target closest enemy as attack target
 *        
 *        3. response to attack signal. and damage enemy when enemy is in front of players
 * Date:2020
 * 
 * Verstion: 0.1
 * 
 * Modify Recoder;
 *  
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Global;
using Kernal;

namespace Control {
    public class Ctrl_PlayerAttack : BaseControl
    {
        public float _FloMinAttackDistance = 5.0f; //minimum attack range (player target enemy range)
        public float _FloPlayerRoataionSpeed = 1.0f; //rotation speed
        public float _FloAttackRange = 2.0f; //attack range

        //about magic values
        public float FloAttackAreaByMagicA = 4f; //magic a attack distance
        public float FloAttackAreaByMagicB = 8f; //magic b attack distance
        public int IntAttackPowerMultipleByMagicA = 5;  //increase attack damage 
        public int IntAttackPowerMultipleByMagicB = 20;  //increase attack damage 

        private List<GameObject> _LisEnemies; //list of enemies
        private Transform _TraNearestEnemy;//get closest enemy postion
        private float _FloMaxDistance = 10.0f;      //enemy and player the maximum distance
      



        private void Awake()
        {
//#if UNITY_ANDROID || UNITY_IPHONE
            //register event(Multicast delegation) : player attack inputs by screen buttom
            Ctrl_PlayerAttackInputByET.evePlayerControl += ResponseBasicAttack;
            Ctrl_PlayerAttackInputByET.evePlayerControl += ResponseMagicTrickA;
            Ctrl_PlayerAttackInputByET.evePlayerControl += ResponseMagicTrickB;
//#endif
            //register event(Multicast delegation) : player attack inputs
           //Ctrl_PlayerAttackInputByKey.evePlayerControl += ResponseBasicAttack;
           //Ctrl_PlayerAttackInputByKey.evePlayerControl += ResponseMagicTrickA;
           //Ctrl_PlayerAttackInputByKey.evePlayerControl += ResponseMagicTrickB;
        }

        private void Start()
        {
            //Collection initialization
            _LisEnemies = new List<GameObject>();

            //put nearby enemies in a enemy array
            StartCoroutine("PutNearByEnemyToArray");

            //player will target closest enemy as attack target
            StartCoroutine("PlayerRotationEnemy");
        }

#region response to attacks signal
        /// <summary>
        /// responding basic attack
        /// </summary>
        public void ResponseBasicAttack(string controlType)
        {
            if (controlType == GlobalParameter.INPUT_MGR_ATTACKNAME_BASIC)
            {
                //play basic attack animation
                Ctrl_PlayerAnimation.Instance.SetCurrentAtionState(PlayerActionState.BasicAttack);
                //Deal damage to specific enemies
               // if(UnityHelper.GetInstance().GetSmallTime(GlobalParameter.INTERVAL_TIME_0DOT1))
               // {
                    AttackEnemyByBasic();
               // }

            }
        }

        /// <summary>
        /// responding magic trick a
        /// </summary>
        public void ResponseMagicTrickA(string controlType)
        {
            if (controlType == GlobalParameter.INPUT_MGR_ATTACKNAME_MAGICTRICK_A)
            {
                //play MagicTrickA attack animation
                Ctrl_PlayerAnimation.Instance.SetCurrentAtionState(PlayerActionState.MagicTrickA);
                //Deal damage to specific enemies
       
                StartCoroutine("AttackEnemyByMagicA");

            }
        }


        /// <summary>
        /// responding magic trick b
        /// </summary>
        public void ResponseMagicTrickB(string controlType)
        {
            if (controlType == GlobalParameter.INPUT_MGR_ATTACKNAME_MAGICTRICK_B)
            {
                //play MagicTrickB attack animation
                Ctrl_PlayerAnimation.Instance.SetCurrentAtionState(PlayerActionState.MagicTrickB);
                //Deal damage to specific enemies
                StartCoroutine("AttackEnemyByMagicB");
            }
        }
#endregion

        //1. put nearby enemies in a enemy array
        IEnumerator PutNearByEnemyToArray()
        {
            while(true)
            {
                yield return new WaitForSeconds(GlobalParameter.INTERVAL_TIME_2);
                // 1.1 get all enemy and put them in an array
                GetEnemiesToArray();
                // 1.2 check enemy array and find out the closest enemy
                GetNearestEnemy();
            }
   

        }
        /// <summary>
        ///  get all enemy and put them in an array
        ///  and check is enemy is alive
        /// </summary>
        public void GetEnemiesToArray()
        {
            //empty enemy list
            _LisEnemies.Clear();
            GameObject[] GoEnemies = GameObject.FindGameObjectsWithTag(Tag.Tag_Enemy);
            foreach(GameObject goItem in GoEnemies)
            {
                //check the enemy is alive
                Ctrl_BaseEnemyProperty enemy = goItem.GetComponent<Ctrl_BaseEnemyProperty>();
                //if (enemy && enemy.IsAlive)
                if (enemy && enemy.CurrentState != EnemyState.Dead)
                {
                    _LisEnemies.Add(goItem);
                }
          
            }
        }//function end
        /// <summary>
        /// check enemy array and find out the closest enemy
        /// </summary>
        private void GetNearestEnemy()
        {
            if(_LisEnemies != null && _LisEnemies.Count >= 1)
            {
                foreach (GameObject goEnemy in _LisEnemies)
                {
                    float floDistance = Vector3.Distance(this.gameObject.transform.position, goEnemy.transform.position);
                    if (floDistance < _FloMaxDistance)
                    {
                        _FloMaxDistance = floDistance;
                        _TraNearestEnemy = goEnemy.transform;
                    }
                }//foreach end
            }
        }//getnearstenemy() end

        //player will target closest enemy as attack target
        IEnumerator PlayerRotationEnemy()
        {
            while(true)
            {
                yield return new WaitForSeconds(GlobalParameter.INTERVAL_TIME_0DOT5);
               if(_TraNearestEnemy != null 
                    && Ctrl_PlayerAnimation.Instance.CurrentActionState
                    == PlayerActionState.Idle)
                {
                    float floDistance = Vector3.Distance(this.gameObject.transform.position, _TraNearestEnemy.position);

                    if(floDistance < _FloMinAttackDistance)
                    {
                        //minimum attack range
                        //this.transform.LookAt(_TraNearestEnemy);
                        UnityHelper.GetInstance().FaceToGoal(this.gameObject.transform,_TraNearestEnemy,_FloPlayerRoataionSpeed);
                    }
         
                }
            }
        }

        /// <summary>
        /// use basic attack to damage enemy
        /// </summary>
        private void AttackEnemyByBasic()
        {
            base.AttackEnemy(_LisEnemies, _TraNearestEnemy, _FloAttackRange, 1);
        }

        /// <summary>
        /// magic a
        /// </summary>
        IEnumerator AttackEnemyByMagicA()
        {
            yield return new WaitForSeconds(GlobalParameter.INTERVAL_TIME_1);
            base.AttackEnemy(_LisEnemies, _TraNearestEnemy, FloAttackAreaByMagicA, IntAttackPowerMultipleByMagicA,false);
        }

        /// <summary>
        /// magic b
        /// </summary>
        IEnumerator AttackEnemyByMagicB()
        {
            yield return new WaitForSeconds(GlobalParameter.INTERVAL_TIME_1);
            base.AttackEnemy(_LisEnemies, _TraNearestEnemy, FloAttackAreaByMagicB, IntAttackPowerMultipleByMagicB);
        }
    }//class end
}


