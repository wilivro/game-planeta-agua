using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class EscovarDentes : MiniGameEscolhas {

	public void OnClickRun(){

		bool torneiraAberta = false;
		int past = 0;
		int pastpast = 0;

		score = 0;

		for(int i = 0; i < itens.Count; i++) {
			if(itens[i].value == 1){
				torneiraAberta = true;
			}
			if((pastpast == 1 || past == 1) && itens[i].value == 4){
				torneiraAberta = false;
			} else {
				if(itens[i].value == template[i]){
					score += 10;
				}
			}
			if(i > 0){
				pastpast = itens[i-1].value;
			}
			past = itens[i].value;
		}

		int _score = PlayerPrefs.GetInt("Score");
		PlayerPrefs.SetInt("Score", score+_score);
		PlayerPrefs.Save();

		if(giveItem != null)
			Player.inventory.Add(giveItem);

		StartCoroutine(End(torneiraAberta));

	}
}