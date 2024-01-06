
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_SkillTreeSlot : MonoBehaviour , IPointerEnterHandler , IPointerExitHandler //,ISaveManager
{
    private Image skillImage;
    [SerializeField] UI_SkillToolTip skillToolTip;

    [SerializeField] private int skillCost;
    [SerializeField] private string skillName;
    [TextArea]
    [SerializeField] private string skillDescription;
    [SerializeField] private Color lockedSkillColor;


    public bool unlocked;

    [SerializeField] private UI_SkillTreeSlot[] shouldBeUnlocked;
    [SerializeField] private UI_SkillTreeSlot[] shouldBeLocked;

    private void OnValidate()
    {
        gameObject.name = "SkillTreeSlot_UI - " + skillName;
    }

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() => UnlockSkillSlot());
    }

    private void Start()
    {
        skillImage = GetComponent<Image>();

        skillImage.color = lockedSkillColor;

        if (unlocked)
            skillImage.color = Color.white;
    }

    public void UnlockSkillSlot()
    {
        if (UserData.Ins.HaveEnoughMoney(skillCost) == false)
            return;

        for (int i = 0; i < shouldBeUnlocked.Length; i++)
        {
            if (shouldBeUnlocked[i].unlocked == false)
            {
                Debug.Log("Cannot unlock skill");
                return;
            }
        }


        for (int i = 0; i < shouldBeLocked.Length; i++)
        {
            if (shouldBeLocked[i].unlocked == true)
            {
                Debug.Log("Cannot unlock skill");
                return;
            }
        }

        unlocked = true;
        skillImage.color = Color.white;
    }



    public void OnPointerEnter(PointerEventData eventData)
    {
        skillToolTip.ShowToolTip(skillDescription,skillName,skillCost);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        skillToolTip.HideToolTip();
    }

    // public void LoadData(GameData _data)
    // {
    //     if (_data.skillTree.TryGetValue(skillName, out bool value))
    //     {
    //         unlocked = value;
    //     }
    // }

    // public void SaveData(ref GameData _data)
    // {
    //     if (_data.skillTree.TryGetValue(skillName, out bool value))
    //     {
    //         _data.skillTree.Remove(skillName);
    //         _data.skillTree.Add(skillName, unlocked);
    //     }
    //     else
    //         _data.skillTree.Add(skillName, unlocked);
    // }
}
