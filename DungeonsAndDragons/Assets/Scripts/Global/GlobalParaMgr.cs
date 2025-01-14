﻿/*
 * Title:"Dungoen and dragons"
 *      
 *      Common layer: global value Manager
 *      
 * Description:
 *          Cross-scenario global value passing
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


namespace Global
{
    public static class GlobalParaMgr
    {
        //next scenes name
        public static ScenesEnum NextScenesName = ScenesEnum.LoadingScenes;

        //get user name
        public static string PlayerName = "";

        //get player type
        public static PlayerType PlayerTypes = PlayerType.Warrior;

        //player type(new game or continue)
        public static CurrentGameType CurGameType = CurrentGameType.None;
    }
}


