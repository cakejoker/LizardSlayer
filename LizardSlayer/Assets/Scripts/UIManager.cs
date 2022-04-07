using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;

    [SerializeField]
    private GameObject tooltip;

    private Player player;
    private MonsterGenerator generator;
    private GameObject panel;

    private Text tooltipText;
    private Text Hptxt;
    private Text Exptxt;
    private Text Statustxt;
    private Text Scoretxt;
    private Text Roundtxt;

    public static UIManager MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<UIManager>();
            }
            return instance;
        }
        set
        {
            instance = value;
        }
    }

    private void Awake()
    {
        tooltipText = tooltip.GetComponentInChildren<Text>();
    }

    // Start is called before the first frame update
    void Start()
    {
        if(InventoryScript.MyInstance.bag.MyBagScript.IsOpen)
            InventoryScript.MyInstance.OpenClose();

        player = GameObject.Find("Player").GetComponent<Player>();
        Hptxt = GameObject.Find("HpText").GetComponent<Text>();
        Exptxt = GameObject.Find("EXPText").GetComponent<Text>();
        Statustxt = GameObject.Find("StatusLevel").GetComponent<Text>();
        panel = GameObject.Find("UpgradePanel");
        Scoretxt = GameObject.Find("Score").GetComponent<Text>();
        Roundtxt = GameObject.Find("Round").GetComponent<Text>();
        generator = GameObject.Find("MonsterGenerator").GetComponent<MonsterGenerator>();
    }

    // Update is called once per frame
    void Update()
    {
        Exptxt.text = player.NowExp.ToString() + "/" + player.MaxExp.ToString();
        Hptxt.text = player.NowHp.ToString() + "/" + player.MaxHp.ToString();
        Statustxt.text = "HP Level : " + player.HpUpgrade.ToString() + "\n"
            + "Attack Level : " + player.AttackUpgrade.ToString() + "\n"
            + "Ability Point : " + player.abilityPoint.ToString();
        Scoretxt.text = "Score : " + GameManager.MyInstance.score.ToString();
        Roundtxt.text = "Round : " + generator.round;

        if (player.abilityPoint != 0)
        {
            panel.SetActive(true);
        }
        else
        {
            panel.SetActive(false);
        }
    }

    public void UpdateStackSize(IClickable clickable)
    {
        if(clickable.MyCount > 1)
        {
            Debug.Log(clickable.MyCount.ToString());
            clickable.MyStackText.text = clickable.MyCount.ToString();
            clickable.MyStackText.color = Color.white;
            clickable.MyIcon.color = Color.white;
        }
        else
        {
            clickable.MyStackText.color = new Color(0, 0, 0, 0);
        }
        if(clickable.MyCount == 0)
        {
            clickable.MyIcon.color = new Color(0, 0, 0, 0);
            clickable.MyStackText.color = new Color(0, 0, 0, 0);
        }
    }

    public void ShowTooltip(Vector3 position, IDescribable description)
    {
        tooltip.SetActive(true);
        tooltip.transform.position = position;

        tooltipText.text = description.GetDescription();
    }

    public void HideTooltip()
    {
        tooltip.SetActive(false);
    }
}
