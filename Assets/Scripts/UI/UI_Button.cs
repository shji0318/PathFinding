using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UI_Button : UI_Base
{    
    enum Buttons
    {
        AStarButton,
        JPSButton,
    }
    void Start()
    {
        Bind<Button>(typeof(Buttons));

        Get<Button>((int)Buttons.AStarButton).onClick.AddListener(() =>
        {
            if (MapManager.Map.AStar == false)
                MapManager.Map.InitFindingAlgorithm(new AStar());

            MapManager.Map.AStar = true;
        }
        );
        Get<Button>((int)Buttons.JPSButton).onClick.AddListener(() =>
        {
            if (MapManager.Map.AStar == true)
                MapManager.Map.InitFindingAlgorithm(new JPS());

            MapManager.Map.AStar = false;
        }
        );
        
    }

    

   
}
