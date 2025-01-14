﻿/*
 * Title:"Dungoen and dragons"
 *      
 *      Control layer: use keyboard to move player
 *      
 * Description:
 *         
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

namespace Control
{
    public class Ctrl_PlayerMovingCtrlByKey : BaseControl
    {
//#if UNITY_STANDALONE_WIN ||UNITY_EDITOR
        public float Speed = 5f;
        private CharacterController cc; //character controller 
        private float gravity = 1f;
        // Use this for initialization
        void Start()
        {

            cc = this.gameObject.GetComponent<CharacterController>();
        }

        // Update is called once per frame
        void Update()
        {
            ControlMoving();
        }

        //use keyboard to move
        void ControlMoving()
         {
            //get horizontal and vertical
            float floMovingXPos = Input.GetAxis(GlobalParameter.INPUT_MGR_HORIZONTAL);
            float floMovingYPos = Input.GetAxis(GlobalParameter.INPUT_MGR_VERTICAL);

            if (floMovingXPos != 0 || floMovingYPos != 0)
             {
                if (Ctrl_PlayerAnimation.Instance.CurrentActionState != PlayerActionState.MagicTrickB)
                {
                    //set up character rotation
                    transform.LookAt(new Vector3(transform.position.x - floMovingXPos, transform.position.y, transform.position.z - floMovingYPos));
                }

                //move character
                //transform.Translate(Vector3.forward * Time.deltaTime * 5);
                Vector3 movement = transform.forward * Time.deltaTime * Speed;
                //play run animation
                movement.y -= gravity;
                if (Ctrl_PlayerAnimation.Instance.CurrentActionState == PlayerActionState.Idle ||
                   Ctrl_PlayerAnimation.Instance.CurrentActionState == PlayerActionState.Running)
                {
                    cc.Move(movement);

                    if (UnityHelper.GetInstance().GetSmallTime(GlobalParameter.INTERVAL_TIME_0DOT2))
                    {
                        Ctrl_PlayerAnimation.Instance.SetCurrentAtionState(PlayerActionState.Running);
                    }
                    else if (UnityHelper.GetInstance().GetSmallTime(GlobalParameter.INTERVAL_TIME_0DOT2))
                    {
                        Ctrl_PlayerAnimation.Instance.SetCurrentAtionState(PlayerActionState.Idle);
                    }

                }

            }

        }//ControlMoving end        
//#endif
    }//class end
}

