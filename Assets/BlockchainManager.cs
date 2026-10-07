using UnityEngine;
using TMPro; // For TextMeshPro Input Fields and Text
// NOTE: Make sure the System.Security.Cryptography and System.Text usings are GONE.

public class BlockchainManager : MonoBehaviour
{
    // These slots will appear in the Unity Inspector
    public TMP_InputField senderInput;
    public TMP_InputField receiverInput;
    public TMP_InputField amountInput;

    // The Text panel where the final hash will be shown
    public TMP_Text hashOutputText;

    // This is the function the button will call
    public void GenerateBlockHash()
    {
        // 1. Get the data from the input fields
        string sender = senderInput.text;
        string receiver = receiverInput.text;
        string amount = amountInput.text;

        // 2. Combine the data into a single string (the "Block Data")
        string blockData = sender + receiver + amount;

        // 3. Generate the SHA-256 Hash
        string newHash = HashUtility.CalculateSHA256(blockData);

        // 4. Update the UI
        hashOutputText.text = "UNIQUE HASH ID:\n" + newHash;
    }
}
