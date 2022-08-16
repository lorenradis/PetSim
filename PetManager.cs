using UnityEngine;
using System.Collections;
using System.Collections.Generic;
[System.Serializable]
public class PetManager  {

/*
what is the pet manager responsible for?
keeping a list of all available pet templates, yeah?
templates are used for instantiating specific instances of pets
does pet manager also keep track of the player's roster of pets?
is each pet a petinfo specifically declared in the pet manager as a private or public instance of the class?

spawn regions will have a list of templates to spawn, right?

we'll have a dedicated scene for choosing your starter which will... i don't know.. something.

the player will be presented with a graphical representation of the 3 pets.
*/

    public List<PetInfo> petTemplates = new List<PetInfo>();

    public List<PetInfo> currentPets = new List<PetInfo>();

    public PetInfo Bulbos;
    public PetInfo Charby;
    public PetInfo Squirt;

    public void SetupPets()
    {
        Bulbos = new PetInfo("Bulbos", 18, 12, 6, AffinityManager.grassAffinity, 
        "Strong and intelligent, but far from the fastest of beasts - Bulbos are most at home in the forests and other areas full of green growing things.");
        Charby = new PetInfo("Charby", 12, 6, 18, AffinityManager.fireAffinity,
        "Fast and powerful, but not the brightest of bulbs - if you'll forgive the pun - Charby are most at home in sweltering temperatures and dry climes");
        Squirt = new PetInfo("Squirt", 6, 18, 12, AffinityManager.waterAffinity,
        "Cunning and quick, but a tad lacking in the brawn department - Squirt are most at home in lakes, rivers, oceans and wetlands of the world");

        petTemplates.Add(Bulbos);
        petTemplates.Add(Charby);
        petTemplates.Add(Squirt);
    }

    public void AddPetToList(PetInfo newPet)
    {
        currentPets.Add(newPet);
    }

}
