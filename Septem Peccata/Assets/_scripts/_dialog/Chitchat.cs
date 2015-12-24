using UnityEngine;
using System.Collections;
using UnityEngine.UI;

//Classe base para a criação de diferentes NPCs com diferentes falas
//maneira de optimizar esta shit: ler texto a partir de um ficheiro (carrega o texto só quando fala com a personagem especifica)
public class Chitchat {

    private Main main;

    //Texto da primeira personagem.
    private ArrayList selfDialog = new ArrayList(2);
    private string[] suggestionDialog = { "It's dark in here, I should find some light.\n(press enter)" };
    private string[] doneDialog = { "press L to turn on the candle." };

    private ArrayList charaterDialog = new ArrayList(2);

    int clicks = 0;

    //GUI
    private GameObject dialogBox;
    private GameObject screenText;

    private Text dialogText;
  
    public Chitchat(Main main, Main.NPCs character, GameObject dialogBox, GameObject screenText)
    {
        this.main = main;
        clicks = 0;

        //definição do chat para cada personagem, como só há uma só há isto bjs (voice would be cool)
        if (character == Main.NPCs.meMyselfAndI)
            charaterDialog = selfDialog;

        this.dialogBox = dialogBox;
        this.screenText = screenText;

        dialogText = dialogBox.transform.FindChild("dialog text").GetComponent<Text>();

        selfDialog.Add(suggestionDialog);
        selfDialog.Add(doneDialog);
    }
	
	public void Update () {

        //só é lido o input se o jogador se encontrar na área de colisão maior do Chitchat ou quando a caixa de dialogo está ativa
        if (screenText != null && (screenText.activeSelf || dialogBox.activeSelf))
        {
            GetInput();
        }
    }

    private void GetInput()
    {
        //se o jogador decidir interagir com o Chitchat é alterado um booleano na main que indica que o jogador está a falar, ou seja, 
        //não se pode movimentar etc e é ligada a caixa de dialogo
        if (Input.GetKeyDown(KeyCode.E) && !main.Chatting)
        {
            screenText.SetActive(false);
            dialogBox.SetActive(true);
            main.Chatting = true;
        }

        /*Se a caixa de diálogo estiver ativa (o jogador está a falar) é lido o input da tecla enter que é usado para prosseguir as mensagens do Chitchat. Dependendo se tem um quest ativo
        ou não, irão ser apresentadas as mensagens de dialogo de sugestão do quest ou a mensagem default. Quando ainda não existem cliques é apresentada a primeira mensagem do array (clicks = 0)*/
        if (dialogBox.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                clicks++;
                
                if (clicks != charaterDialog.Count-1 && main.ActiveQuest == Main.QuestType.none)
                    setText(clicks, (string[])charaterDialog[0]);
                
                else if(clicks == charaterDialog.Count-1 && main.ActiveQuest == Main.QuestType.none)
                {
                    clicks = 0;

                    dialogBox.SetActive(false);
                    main.Chatting = false;

                    //o jogador aceita o quest e este é guardado na main
                    main.ActiveQuest = Main.QuestType.lamp;
                }

                else
                    setText(clicks, (string[])charaterDialog[1]);
            }

            else if (clicks == 0)
            {
                if (main.ActiveQuest == Main.QuestType.none)
                    setText(clicks, (string[])charaterDialog[0]);
                else
                    setText(0, (string[])charaterDialog[1]);
            }

        }
    }

    public void buttonAccept()
    {
             
    }

    public void buttonCancel()
    {
        clicks = 0;

        dialogBox.SetActive(false);
        main.Chatting = false;
    }

    private void setText(int click, string[] text)
    {
        //enquanto existirem mensagens no array, são apresentadas conforme o número de vezes que o jogador clica no enter
        if (click >= 0 && click < text.Length)
            dialogText.text = text[click];

        /*se o jogador ainda não tiver aceite um quest são apresentados os botões para aceitar/cancelar na penultima mensagem do Chitchat. Caso contrário quando 
         *chega ao número máximo de cliques a mensagem desaparece*/
        
    }

    //ao colidir com a maior área de colisão do Chitchat é apresentada a mensagem para interação, quano o jogador sai da area a mensagem desaparece
    public void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Player") && !dialogBox.activeSelf)
            screenText.SetActive(true);
    }

    public void OnTriggerExit(Collider col)
    {
        screenText.SetActive(false);
    }
}
