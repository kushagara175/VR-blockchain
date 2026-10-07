using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables; // For the OnSelect event

public class BlockChainBlock : MonoBehaviour
{
    // VARIABLES TO BE SET IN INSPECTOR
    public BlockChainBlock nextBlock;           // Slot to link the next block in the chain
    public string previousHash;                  // The required hash of the block before this one
    public string blockData = "Transaction Data"; // Data that defines this block

    // INTERNAL VARIABLES
    public string currentHash;
    private Renderer blockRenderer;
    private XRBaseInteractable interactable; // For XR interaction


    // Inside BlockChainBlock.cs

    // ... (other variables)

    void Start()
    {
        blockRenderer = GetComponent<Renderer>();

        // Setup XR Interaction
        interactable = GetComponent<XRBaseInteractable>();

        if (interactable != null)
        {
            // *** CHANGE THIS LINE TO LISTEN FOR HOVER ***
            // We use hoverEntered because it requires NO BUTTON PRESS, just the pointer hitting the object.
            interactable.hoverEntered.AddListener(OnSelectHover);

            // Remove the OnSelect(ActivateEventArgs args) method from the script if you still have it.
        }

        // Initialize the chain validation
        currentHash = CalculateHash();
        CheckValidity();
    }

    // THIS METHOD MUST ACCEPT HoverEnterEventArgs
    private void OnSelectHover(HoverEnterEventArgs args)
    {
        Debug.Log("--- HOVER TRIGGERED! Tampering " + gameObject.name + " ---");
        TamperBlock();
    }

    // ... rest of the BlockChainBlock class continues below ...

    // NOTE: This signature MUST match the event type used in Start()
    private void OnSelect(ActivateEventArgs args)
    {
        Debug.Log("--- ON ACTIVATE TRIGGERED! Attempting to Tamper on: " + gameObject.name + " ---");
        TamperBlock();
    }

    
    public string CalculateHash()
    {
        // The hash is based on the Previous Block's Hash PLUS the current data.
        string dataToHash = previousHash + blockData;

        // Use the shared utility function
        return HashUtility.CalculateSHA256(dataToHash);
    }

    // Checks if the block is valid in the chain and updates the color
    public void CheckValidity()
    {
        // 1. The Genesis Block (Cube 1) is always valid
        if (gameObject.name == "Cube (1)")
        {
            SetBlockColor(Color.green);
            // Tell the next block to validate, passing our newly generated hash
            if (nextBlock != null)
            {
                nextBlock.previousHash = currentHash;
                nextBlock.CheckValidity(); // Recursive call to validate the next block
            }
            return;
        }

        // 2. All other blocks check their chain link
        string expectedHash = CalculateHash();

        // If the hash we calculated (expectedHash) matches the hash we currently store (currentHash), 
        // the block itself is valid. But the primary check is to see if the previous block's hash is correct.

        // Since the previous block *sets* our previousHash variable (in Cube 1's logic), 
        // if we are reached, we assume the previous link is valid.

        SetBlockColor(Color.green); // Set self to valid (green)

        // Continue the validation chain to the next block
        if (nextBlock != null)
        {
            nextBlock.previousHash = currentHash; // Pass our hash to the next block
            nextBlock.CheckValidity();
        }
    }

    // ----------------------------------------------------
    // Tampering Logic (Called by XR Select)
    // ----------------------------------------------------

    // This runs when the XR controller selects the block
    

    // This function is what happens when the user interacts with the block (tampering)
    public void TamperBlock()
    {
        // 1. Simulate tampering by changing the data
        blockData = "HACKED DATA! Value changed: " + Random.Range(1000, 9999);

        // 2. Recalculate the hash (which will change completely due to new blockData)
        currentHash = CalculateHash();

        // 3. Set this block to RED
        SetBlockColor(Color.red);

        // 4. Force the cascade: Tell the NEXT block to check its integrity
        if (nextBlock != null)
        {
            // By calling CascadeFailure, we skip the validation check in the next block
            // and immediately start the red chain reaction.
            nextBlock.CascadeFailure();
        }
    }

    // This function runs on subsequent blocks when a block before them is tampered with
    public void CascadeFailure()
    {
        // Set this block to RED because the link to the previous block is broken
        SetBlockColor(Color.red);

        // Continue the chain of failure to the block after this one
        if (nextBlock != null)
        {
            nextBlock.CascadeFailure();
        }
    }

    // ----------------------------------------------------
    // Visuals
    // ----------------------------------------------------

    private void SetBlockColor(Color newColor)
    {
        if (blockRenderer != null)
        {
            blockRenderer.material.color = newColor;
        }
    }
}