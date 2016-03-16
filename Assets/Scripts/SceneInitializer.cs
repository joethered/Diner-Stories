using UnityEngine;
using System.Collections;


/// <summary>
/// Uses a Behaviour Tree to setup a scene
/// </summary>
public class SceneInitializer {

    public Selector setupSpeedDateTree()
    {
        Selector root = new Selector("Speed Date Setup");
        {
            //Conclusion Plan
            Sequence toConclusionPlan = new Sequence("To Conclusion Plan");
            {
                Check isConclusionAct = new Check("isThirdScene");
                Selector conclusionPlan = new Selector("Conclusion Plan");
                {
                    Sequence robberAndWidow = new Sequence("The Robber and the Widow");
                    {
                        Check isRobberPlotThreadActive = new Check("isRobberPlotThreadActive");
                        Check isWidowedPlotThreadActive = new Check("isWidowedPlotThreadActive");
                        Action setPlayerAsWidow = new Action("setPlayerAsWidow");
                        Action setPlayerAsDater1 = new Action("setPlayerAsDater1");
                        Action setRobberAsDater2 = new Action("setRobberAsDater2");
                        robberAndWidow.children.Add(isRobberPlotThreadActive);
                        robberAndWidow.children.Add(isWidowedPlotThreadActive);
                        robberAndWidow.children.Add(setPlayerAsWidow);
                        robberAndWidow.children.Add(setPlayerAsDater1);
                        robberAndWidow.children.Add(setRobberAsDater2);
                    }
                    Sequence robberOnADate = new Sequence("The Robber Goes on a Date");
                    {
                        Check isRobberPlotThreadActive = new Check("isRobberPlotThreadActive");
                        Selector whoIsTheRobber = new Selector("Who will play the Robber");
                        {
                            Sequence robberWasPlayer = new Sequence("The robber was the player");
                            {
                                Check wasRobberPlayer = new Check("wasRobberPlayer");
                                Action setNewAsDater1 = new Action("setNewAsDater1");  // Was Existing
                                Action setPlayerAsDater1 = new Action("setPlayerAsDater1");
                                Action setRobberAsDater2 = new Action("setRobberAsDater2");
                                robberWasPlayer.children.Add(wasRobberPlayer);
                                robberWasPlayer.children.Add(setNewAsDater1);              // Was Existing
                                robberWasPlayer.children.Add(setPlayerAsDater1);
                                robberWasPlayer.children.Add(setRobberAsDater2);
                            }
                            Sequence robberWasNotPlayer = new Sequence("The robber was not the player");
                            {
                                Action setRobberAsDater1 = new Action("setRobberAsDater1");
                                Action setPlayerAsDater1 = new Action("setPlayerAsDater1");
                                Action setNewAsDater2 = new Action("setNewAsDater2"); // Was Existing
                                robberWasNotPlayer.children.Add(setRobberAsDater1);
                                robberWasNotPlayer.children.Add(setPlayerAsDater1);
                                robberWasNotPlayer.children.Add(setNewAsDater2);           // Was Existing
                            }
                            
                            whoIsTheRobber.children.Add(robberWasPlayer);
                            whoIsTheRobber.children.Add(robberWasNotPlayer);
                        }
                        robberOnADate.children.Add(isRobberPlotThreadActive);
                        robberOnADate.children.Add(whoIsTheRobber);
                    }
                    Sequence widowOnADate = new Sequence("The Widow Goes on a Date");
                    {
                        Check wasWidowPlayer = new Check("wasWidowPlayer");
                        Action setNewAsDater1 = new Action("setNewAsDater1");         // Was Existing
                        Action setPlayerAsDater1 = new Action("setPlayerAsDater1");
                        Action setWidowAsDater2 = new Action("setWidowAsDater2");
                        widowOnADate.children.Add(wasWidowPlayer);
                        widowOnADate.children.Add(setNewAsDater1);                         // Was Existing
                        widowOnADate.children.Add(setPlayerAsDater1);
                        widowOnADate.children.Add(setWidowAsDater2);
                    }
                    Sequence playerWidow = new Sequence("The Player is the Widow");
                    {
                        Action setWidowAsDater1 = new Action("setWidowAsDater1");
                        Action setPlayerAsDater1 = new Action("setPlayerAsDater1");
                        Action setNewAsDater2 = new Action("setNewAsDater2");         // Was Existing
                        widowOnADate.children.Add(setWidowAsDater1);
                        widowOnADate.children.Add(setPlayerAsDater1);
                        widowOnADate.children.Add(setNewAsDater2);                         // Was Existing
                    }
                    conclusionPlan.children.Add(robberAndWidow);
                    conclusionPlan.children.Add(robberOnADate);
                    conclusionPlan.children.Add(widowOnADate);
                    conclusionPlan.children.Add(playerWidow);
                }
                toConclusionPlan.children.Add(isConclusionAct);
                toConclusionPlan.children.Add(conclusionPlan);
            }
            

            //Climactic Plan
            Sequence toClimacticPlan = new Sequence("To Climactic Plan");
            {
                Check isClimacticScene = new Check("isSecondScene");
                Selector climaticPlan = new Selector("Climactic Plan");
                {
                    Sequence datingWithRobber = new Sequence("Dater With Robber");
                    {
                        Check isRobberPlotThreadActive = new Check("isRobberPlotThreadActive");
                        Selector whoIsTheRobber = new Selector("Who will play the Robber");
                        {
                            Sequence robberWasPlayer = new Sequence("The robber was the player");
                            {
                                Check wasRobberPlayer = new Check("wasRobberPlayer");
                                Action setRobberAsDater1 = new Action("setRobberAsDater1");
                                Action setPlayerAsDater1 = new Action("setPlayerAsDater1");
                                Action setNewAsDater2 = new Action("setNewAsDater2");
                                robberWasPlayer.children.Add(wasRobberPlayer);
                                robberWasPlayer.children.Add(setRobberAsDater1);
                                robberWasPlayer.children.Add(setPlayerAsDater1);
                                robberWasPlayer.children.Add(setNewAsDater2);
                            }

                            whoIsTheRobber.children.Add(robberWasPlayer);
                        }
                        datingWithRobber.children.Add(isRobberPlotThreadActive);
                        datingWithRobber.children.Add(whoIsTheRobber);
                        
                    }
                    Sequence widowedMovesOn = new Sequence("Widowed Moves On");
                    { 
                        Check isWidowedPlotThreadActive = new Check("isWidowedPlotThreadActive");
                        Selector whoIsWidow = new Selector("Who will play the Widow");
                        {
                            Sequence widowWasPlayer = new Sequence("The widow was the player");
                            {
                                Check wasWidowPlayer = new Check("wasWidowPlayer");
                                Action setWidowAsDater1 = new Action("setWidowAsDater1");
                                Action setPlayerAsDater1 = new Action("setPlayerAsDater1");
                                Action setNewAsDater2 = new Action("setNewAsDater2");
                                widowWasPlayer.children.Add(wasWidowPlayer);
                                widowWasPlayer.children.Add(setWidowAsDater1);
                                widowWasPlayer.children.Add(setPlayerAsDater1);
                                widowWasPlayer.children.Add(setNewAsDater2);
                            }
                            
                            whoIsWidow.children.Add(widowWasPlayer);
                        }
                        
                        widowedMovesOn.children.Add(isWidowedPlotThreadActive);
                        widowedMovesOn.children.Add(whoIsWidow);
                        
                    }
                    climaticPlan.children.Add(datingWithRobber);
                    climaticPlan.children.Add(widowedMovesOn);
                }
                toClimacticPlan.children.Add(isClimacticScene);
                toClimacticPlan.children.Add(climaticPlan);
            }
            //Exposition Plan
            Action createRandomSpeedDateSetup = new Action("createRandomSpeedDateSetup");
            root.children.Add(toConclusionPlan);
            root.children.Add(toClimacticPlan);
            root.children.Add(createRandomSpeedDateSetup);
        }
        return root;
    }

    public Selector setupRobberyTree()
    {
        Selector root = new Selector("Robbery Setup");
        {
            //Conclusion Plan
            Sequence toConclusionPlan = new Sequence("To Conclusion Plan");
            {
                Check isConclusionAct = new Check("isThirdScene");
                Selector conclusionPlan = new Selector("Conclusion Plan");
                {
                    Sequence widdowGoneMad = new Sequence("Widow Gone Mad");
                    {
                        Check isWidowedPlotThreadActive = new Check("isWidowedPlotThreadActive");
                        Selector whoIsTheWidow = new Selector("Who will play the Widow");
                        {
                            Sequence widowWasPlayer = new Sequence("The widow was the Player");
                            {
                                Check wasWidowPlayer = new Check("wasWidowPlayer");
                                Action setNewAsServer = new Action("setNewAsServer");     // Was Existing
                                Action setPlayerAsServer = new Action("setPlayerAsServer");
                                Action setWidowAsRobber = new Action("setWidowAsRobber");
                                Action setNewAsPatron = new Action("setNewAsPatron");     // Was Existing
                                widowWasPlayer.children.Add(wasWidowPlayer);
                                widowWasPlayer.children.Add(setNewAsServer);                   // Was Existing
                                widowWasPlayer.children.Add(setPlayerAsServer);
                                widowWasPlayer.children.Add(setWidowAsRobber);
                                widowWasPlayer.children.Add(setNewAsPatron);                   // Was Existing
                            }
                            Sequence widowWasNotPlayer = new Sequence("The widow was not the player");
                            {
                                Action setWidowAsRobber = new Action("setWidowAsRobber");
                                Action setPlayerAsRobber = new Action("setPlayerAsRobber");
                                Action setNewAsServer = new Action("setNewAsServer");     // Was Existing
                                Action setNewAsPatron = new Action("setNewAsPatron");     // Was Existing
                                widowWasNotPlayer.children.Add(setWidowAsRobber);
                                widowWasNotPlayer.children.Add(setPlayerAsRobber);
                                widowWasNotPlayer.children.Add(setNewAsServer);                // Was Existing
                                widowWasNotPlayer.children.Add(setNewAsPatron);                // Was Existing
                            }
                            whoIsTheWidow.children.Add(widowWasPlayer);
                            whoIsTheWidow.children.Add(widowWasNotPlayer);
                        }
                        widdowGoneMad.children.Add(isWidowedPlotThreadActive);
                        widdowGoneMad.children.Add(whoIsTheWidow);
                    }
                    Sequence starCrossedLovers = new Sequence("Star Crossed Lovers");
                    {
                        Check isLoversThreadActive = new Check("isLoversThreadActive");
                        Sequence statCrossedSetup = new Sequence("Star Crossed Setup");
                        {
                            Action setNewAsRobber = new Action("setNewAsRobber");         // Was Existing
                            Action setPlayerAsRobber = new Action("setPlayerAsRobber");
                            Action setLover1AsServer = new Action("setLover1AsServer");
                            Action setLover2AsPatron = new Action("setLover2AsPatron");
                            statCrossedSetup.children.Add(setNewAsRobber);                     // Was Existing
                            statCrossedSetup.children.Add(setPlayerAsRobber);
                            statCrossedSetup.children.Add(setLover1AsServer);
                            statCrossedSetup.children.Add(setLover2AsPatron);
                        }
                        starCrossedLovers.children.Add(isLoversThreadActive);
                        starCrossedLovers.children.Add(statCrossedSetup);
                    }
                    Sequence spurnedDater = new Sequence("The Spurned Dater");
                    {
                        Check isSpurnedThreadActive = new Check("isSpurnedThreadActive");
                        Action setSpurnedAsRobber = new Action("setSpurnedAsRobber");
                        Action setNewAsPatron = new Action("setNewAsPatron");             // Was Existing
                        Action setPlayerAsPatron = new Action("setPlayerAsPatron");
                        Action setNewAsServer = new Action("setNewAsServer");             // Was Existing
                        spurnedDater.children.Add(isSpurnedThreadActive);
                        spurnedDater.children.Add(setSpurnedAsRobber);
                        spurnedDater.children.Add(setNewAsPatron);                             // Was Existing
                        spurnedDater.children.Add(setPlayerAsPatron);
                        spurnedDater.children.Add(setNewAsServer);                             // Was Existing
                    }
                    Sequence doesntMatter = new Sequence("Doesnt Matter");
                    {
                        Action setNewAsRobber = new Action("setNewAsRobber");             // Was Existing
                        Action setPlayerAsRobber = new Action("setPlayerAsRobber");
                        Action setNewAsServer = new Action("setNewAsServer");             // Was Existing
                        Action setNewAsPatron = new Action("setNewAsPatron");             // Was Existing
                        doesntMatter.children.Add(setNewAsRobber);                             // Was Existing
                        doesntMatter.children.Add(setPlayerAsRobber);
                        doesntMatter.children.Add(setNewAsServer);                             // Was Existing
                        doesntMatter.children.Add(setNewAsPatron);                             // Was Existing

                    }
                    conclusionPlan.children.Add(widdowGoneMad);
                    conclusionPlan.children.Add(starCrossedLovers);
                    conclusionPlan.children.Add(spurnedDater);
                    conclusionPlan.children.Add(doesntMatter);
                }
                toConclusionPlan.children.Add(isConclusionAct);
                toConclusionPlan.children.Add(conclusionPlan);
            }


            //Climactic Plan
            Sequence toClimacticPlan = new Sequence("To Climactic Plan");
            {
                Check isClimacticScene = new Check("isSecondScene");
                Selector climaticPlan = new Selector("ClimacticPlan");
                {
                    Sequence starCrossedLovers = new Sequence("Star Crossed Lovers");
                    {
                        Check isLoversThreadActive = new Check("isLoversThreadActive");
                        Sequence statCrossedSetup = new Sequence("Star Crossed Setup");
                        {
                            Action setNewAsRobber = new Action("setNewAsRobber");
                            Action setPlayerAsRobber = new Action("setPlayerAsRobber");
                            Action setLover1AsServer = new Action("setLover1AsServer");
                            Action setLover2AsPatron = new Action("setLover2AsPatron");
                            statCrossedSetup.children.Add(setNewAsRobber);
                            statCrossedSetup.children.Add(setPlayerAsRobber);
                            statCrossedSetup.children.Add(setLover1AsServer);
                            statCrossedSetup.children.Add(setLover2AsPatron);
                        }
                        starCrossedLovers.children.Add(isLoversThreadActive);
                        starCrossedLovers.children.Add(statCrossedSetup);
                    }
                    Sequence spurnedDater = new Sequence("The Spurned Dater");
                    {
                        Check isSpurnedThreadActive = new Check("isSpurnedThreadActive");
                        Action setSpurnedAsRobber = new Action("setSpurnedAsRobber");
                        Action setNewAsPatron= new Action("setNewAsPatron");
                        Action setNewAsServer = new Action("setNewAsServer");
                        Action setPlayerAsServer = new Action("setPlayerAsServer");
                        spurnedDater.children.Add(isSpurnedThreadActive);
                        spurnedDater.children.Add(setSpurnedAsRobber);
                        spurnedDater.children.Add(setNewAsPatron);
                        spurnedDater.children.Add(setNewAsServer);
                        spurnedDater.children.Add(setPlayerAsServer);
                    }
                    Sequence madWidowPlayer = new Sequence("The Mad Widow Player");
                    {
                        Check wasWidowPlayer = new Check("wasWidowPlayer");
                        Action setWidowAsRobber = new Action("setWidowAsRobber");
                        Action setNewAsServer = new Action("setNewAsServer");
                        Action setPlayerAsServer = new Action("setPlayerAsServer");
                        Action setNewAsPatron = new Action("setNewAsPatron");
                        madWidowPlayer.children.Add(wasWidowPlayer);
                        madWidowPlayer.children.Add(setWidowAsRobber);
                        madWidowPlayer.children.Add(setNewAsServer);
                        madWidowPlayer.children.Add(setPlayerAsServer);
                        madWidowPlayer.children.Add(setNewAsPatron);
                    }
                    Sequence madWidow = new Sequence("The Mad Widow");
                    {
                        Action setWidowAsRobber = new Action("setWidowAsRobber");
                        Action setPlayerAsRobber = new Action("setPlayerAsServer");
                        Action setNewAsServer = new Action("setNewAsServer");
                        Action setNewAsPatron = new Action("setNewAsPatron");
                        madWidow.children.Add(setWidowAsRobber);
                        madWidow.children.Add(setPlayerAsRobber);
                        madWidow.children.Add(setNewAsServer);
                        madWidow.children.Add(setNewAsPatron);
                    }
                    climaticPlan.children.Add(starCrossedLovers);
                    climaticPlan.children.Add(spurnedDater);
                    climaticPlan.children.Add(madWidowPlayer);
                    climaticPlan.children.Add(madWidow);
                }
                toClimacticPlan.children.Add(isClimacticScene);
                toClimacticPlan.children.Add(climaticPlan);
            }
            //Exposition Plan
            Action createRandomRobberySetup = new Action("createRandomRobberySetup");
            root.children.Add(toConclusionPlan);
            root.children.Add(toClimacticPlan);
            root.children.Add(createRandomRobberySetup);
        }
        return root;
    }

	public Selector setupFuneralTree(){
		Selector root = new Selector ("Funeral Setup");
		{
			//Conclusion plan
			Sequence toConclusionPlan = new Sequence("To Conclusion Plan");
			{
				Check isConclusionAct = new Check ("isThirdScene");
				Selector conclusionPlan = new Selector ("Conclusion Plan");
				{
					Sequence robberAndLover = new Sequence ("The Robber and the Lover");
					{
						Check isRobberPlotThreadActive = new Check("isRobberPlotThreadActive");
						Check isLoverPlotThreadActive = new Check("isLoverPlotThreadActive");
                        Action setLover1AsWidow = new Action("setLover1AsWidow");
                        Action setPlayerAsWidow = new Action("setPlayerAsWidow");
						Action setDateAsRobber = new Action("setDateAsRobber");
                        robberAndLover.children.Add(isRobberPlotThreadActive);
                        robberAndLover.children.Add(isLoverPlotThreadActive);
                        robberAndLover.children.Add(setLover1AsWidow);
                        robberAndLover.children.Add(setPlayerAsWidow);
                        robberAndLover.children.Add(setDateAsRobber);
                    }
					Sequence robberAtFuneral = new Sequence ("Robber at Funeral");
					{
						Check isRobberPlotThreadActive = new Check("isRobberPlotThreadActive");
						Selector whoIsTheRobber = new Selector("Who will play the Robber");
						{
							Sequence robberWasPlayer = new Sequence("The robber was the player");
							{
								Check wasRobberPlayer = new Check("wasRobberPlayer");
								Action setMournerAsRobber = new Action("setMournerAsRobber");
								Action setPlayerAsNew = new Action("setPlayerAsNew");     // Was Existing
                                robberWasPlayer.children.Add(wasRobberPlayer);
								robberWasPlayer.children.Add(setMournerAsRobber);
								robberWasPlayer.children.Add(setPlayerAsNew);                  // Was Existing
                            }
							Sequence robberWasNotPlayer = new Sequence("The robber was not the player");
							{
								Action setPlayerAsRobber = new Action("setPlayerAsRobber");
								Action setMournerAsNew = new Action("setMournerAsNew");   // Was Existing
                                robberWasNotPlayer.children.Add(setPlayerAsRobber);
								robberWasNotPlayer.children.Add(setMournerAsNew);              // Was Existing
                            }
							whoIsTheRobber.children.Add(robberWasPlayer);
							whoIsTheRobber.children.Add(robberWasNotPlayer);
						}
                        robberAtFuneral.children.Add(isRobberPlotThreadActive);
                        robberAtFuneral.children.Add(whoIsTheRobber);
					}
					Sequence LoveratFune = new Sequence("The Lover Goes to Funeral");
					{
						Check wasLoverPlayer = new Check("wasLoverPlayer");
						Action setFuneAsLover = new Action("setFuneAsLover");
						Action setPlayerAsNew = new Action("setPlayerAsNew");             // Was Existing
                        LoveratFune.children.Add(wasLoverPlayer);
						LoveratFune.children.Add(setFuneAsLover);
						LoveratFune.children.Add(setPlayerAsNew);                              // Was Existing
                    }
					Sequence playerLover = new Sequence("The Player is the Widow");
					{
						Action setPlayerAsLover = new Action("setPlayerAsLover");
						Action setFuneAsNew = new Action("setFuneAsNew");                 // Was Existing
                        LoveratFune.children.Add(setPlayerAsLover);
						LoveratFune.children.Add(setFuneAsNew);                                // Was Existing
                    }
					conclusionPlan.children.Add(robberAndLover);
					conclusionPlan.children.Add(robberAtFuneral);
					conclusionPlan.children.Add(LoveratFune);
					conclusionPlan.children.Add(playerLover);
				}
				toConclusionPlan.children.Add(isConclusionAct);
				toConclusionPlan.children.Add(conclusionPlan);
			}
			//Climactic plan
			Sequence toClimacticPlan = new Sequence("To Climactic Plan");
			{
				Check isClimacticScene = new Check ("isSecondScene");
				Selector climaticPlan = new Selector ("ClimacticPlan");
				{
					Sequence RobberatFune = new Sequence ("Robber at Funeral");
					{
						Check isRobberPlotThreadActive = new Check("isRobberPlotThreadActive");
						Selector whoIsTheRobber = new Selector("Who will play the Robber");
						{
							Sequence robberWasPlayer = new Sequence("The robber was the player");
							{
								Check wasRobberPlayer = new Check("wasRobberPlayer");
								Action robberFune = new Action("makeRobberMourn");
								robberWasPlayer.children.Add(wasRobberPlayer);
								robberWasPlayer.children.Add(robberFune);
							}

							Action robberPlayer = new Action("makerRobberPlayer");
							whoIsTheRobber.children.Add(robberWasPlayer);
							whoIsTheRobber.children.Add(robberPlayer);
						}
						RobberatFune.children.Add(isRobberPlotThreadActive);
						RobberatFune.children.Add(whoIsTheRobber);
					}
					Sequence loversthere = new Sequence("Lover is there");
					{
						Check isLoverPlotThreadActive = new Check("isLoverPlotThreadActive");
						Selector whoIsTheLover = new Selector("Who will play the Lover");
						{
							Sequence loverWasPlayer = new Sequence("The lover was the player");
							{
								Check wasloverPlayer = new Check("wasloverPlayer");
								Action loverFune = new Action("makeLoverMourn");
								loverWasPlayer.children.Add(wasloverPlayer);
								loverWasPlayer.children.Add(loverFune);
							}
							Action loverPlayer = new Action("makerloverPlayer");
							whoIsTheLover.children.Add(loverWasPlayer);
							whoIsTheLover.children.Add(loverPlayer);
						}
						loversthere.children.Add(isLoverPlotThreadActive);
						loversthere.children.Add(whoIsTheLover);
					}
					climaticPlan.children.Add(RobberatFune);
					climaticPlan.children.Add(loversthere);
				}
				toClimacticPlan.children.Add(isClimacticScene);
				toClimacticPlan.children.Add(climaticPlan);
			}
			//Exposition Plan
			Action createRandomFuneralSetup = new Action("createRandomFuneralSetup");
			root.children.Add(toConclusionPlan);
			root.children.Add(toClimacticPlan);
			root.children.Add(createRandomFuneralSetup);
		}
		return root;
	}
}
