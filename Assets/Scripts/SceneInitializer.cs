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
                        Action setDateAsRobber = new Action("setDateAsRobber");
                    }
                    Sequence robberOnADate = new Sequence("The Robber Goes on a Date");
                    {
                        Check isRobberPlotThreadActive = new Check("isRobberPlotThreadActive");
                        Selector whoIsTheRobber = new Selector("Who will play the Robber");
                        {
                            Sequence robberWasPlayer = new Sequence("The robber was the player");
                            {
                                Check wasRobberPlayer = new Check("wasRobberPlayer");
                                Action setDateAsRobber = new Action("setDateAsRobber");
                                Action setPlayerAsExisting = new Action("setPlayerAsExisting");
                                robberWasPlayer.children.Add(wasRobberPlayer);
                                robberWasPlayer.children.Add(setDateAsRobber);
                                robberWasPlayer.children.Add(setPlayerAsExisting);
                            }
                            Sequence robberWasNotPlayer = new Sequence("The robber was not the player");
                            {
                                Action setPlayerAsRobber = new Action("setPlayerAsRobber");
                                Action setDateAsExisting = new Action("setDateAsExisting");
                                robberWasNotPlayer.children.Add(setPlayerAsRobber);
                                robberWasNotPlayer.children.Add(setDateAsExisting);
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
                        Action setDateAsWidow = new Action("setDateAsWidow");
                        Action setPlayerAsExisting = new Action("setPlayerAsExisting");
                        widowOnADate.children.Add(wasWidowPlayer);
                        widowOnADate.children.Add(setDateAsWidow);
                        widowOnADate.children.Add(setPlayerAsExisting);
                    }
                    Sequence playerWidow = new Sequence("The Player is the Widow");
                    {
                        Action setPlayerAsWidow = new Action("setPlayerAsWidow");
                        Action setDateAsExisting = new Action("setDateAsExisting");
                        widowOnADate.children.Add(setPlayerAsWidow);
                        widowOnADate.children.Add(setDateAsExisting);
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
                Selector climaticPlan = new Selector("ClimacticPlan");
                {
                    Sequence datingWithRobber = new Sequence("Dater With Robber");
                    {
                        Check isRobberPlotThreadActive = new Check("isRobberPlotThreadActive");
                        Selector whoIsTheRobber = new Selector("Who will play the Robber");
                        {
                            Sequence robberWasPlayer = new Sequence("The robber was the player");
                            {
                                Check wasRobberPlayer = new Check("wasRobberPlayer");
                                Action robberDate = new Action("makeRobberDate");
                                robberWasPlayer.children.Add(wasRobberPlayer);
                                robberWasPlayer.children.Add(robberDate);
                            }

                            Action robberPlayer = new Action("makerRobberPlayer");
                            whoIsTheRobber.children.Add(robberWasPlayer);
                            whoIsTheRobber.children.Add(robberPlayer);
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
                                Action widowDate = new Action("makeWidowDate");
                                widowWasPlayer.children.Add(wasWidowPlayer);
                                widowWasPlayer.children.Add(widowDate);
                            }
                            Action widowPlayer = new Action("widowPlayer");
                            whoIsWidow.children.Add(widowPlayer);
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
            Action createRandomSetup = new Action("createRandomSetup");
            root.children.Add(toConclusionPlan);
            root.children.Add(toClimacticPlan);
            root.children.Add(createRandomSetup);
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
                    Sequence robberAndWidow = new Sequence("The Robber and the Widow");
                    {
                        Check isRobberPlotThreadActive = new Check("isRobberPlotThreadActive");
                        Check isWidowedPlotThreadActive = new Check("isWidowedPlotThreadActive");
                        Action setPlayerAsWidow = new Action("setPlayerAsWidow");
                        Action setDateAsRobber = new Action("setDateAsRobber");
                    }
                    Sequence robberOnADate = new Sequence("The Robber Goes on a Date");
                    {
                        Check isRobberPlotThreadActive = new Check("isRobberPlotThreadActive");
                        Selector whoIsTheRobber = new Selector("Who will play the Robber");
                        {
                            Sequence robberWasPlayer = new Sequence("The robber was the player");
                            {
                                Check wasRobberPlayer = new Check("wasRobberPlayer");
                                Action setDateAsRobber = new Action("setDateAsRobber");
                                Action setPlayerAsExisting = new Action("setPlayerAsExisting");
                                robberWasPlayer.children.Add(wasRobberPlayer);
                                robberWasPlayer.children.Add(setDateAsRobber);
                                robberWasPlayer.children.Add(setPlayerAsExisting);
                            }
                            Sequence robberWasNotPlayer = new Sequence("The robber was not the player");
                            {
                                Action setPlayerAsRobber = new Action("setPlayerAsRobber");
                                Action setDateAsExisting = new Action("setDateAsExisting");
                                robberWasNotPlayer.children.Add(setPlayerAsRobber);
                                robberWasNotPlayer.children.Add(setDateAsExisting);
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
                        Action setDateAsWidow = new Action("setDateAsWidow");
                        Action setPlayerAsExisting = new Action("setPlayerAsExisting");
                        widowOnADate.children.Add(wasWidowPlayer);
                        widowOnADate.children.Add(setDateAsWidow);
                        widowOnADate.children.Add(setPlayerAsExisting);
                    }
                    Sequence playerWidow = new Sequence("The Player is the Widow");
                    {
                        Action setPlayerAsWidow = new Action("setPlayerAsWidow");
                        Action setDateAsExisting = new Action("setDateAsExisting");
                        widowOnADate.children.Add(setPlayerAsWidow);
                        widowOnADate.children.Add(setDateAsExisting);
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
                Selector climaticPlan = new Selector("ClimacticPlan");
                {
                    Sequence datingWithRobber = new Sequence("Dater With Robber");
                    {
                        Check isRobberPlotThreadActive = new Check("isRobberPlotThreadActive");
                        Selector whoIsTheRobber = new Selector("Who will play the Robber");
                        {
                            Sequence robberWasPlayer = new Sequence("The robber was the player");
                            {
                                Check wasRobberPlayer = new Check("wasRobberPlayer");
                                Action robberDate = new Action("makeRobberDate");
                                robberWasPlayer.children.Add(wasRobberPlayer);
                                robberWasPlayer.children.Add(robberDate);
                            }

                            Action robberPlayer = new Action("makerRobberPlayer");
                            whoIsTheRobber.children.Add(robberWasPlayer);
                            whoIsTheRobber.children.Add(robberPlayer);
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
                                Action widowDate = new Action("makeWidowDate");
                                widowWasPlayer.children.Add(wasWidowPlayer);
                                widowWasPlayer.children.Add(widowDate);
                            }
                            Action widowPlayer = new Action("widowPlayer");
                            whoIsWidow.children.Add(widowPlayer);
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
            Action createRandomSetup = new Action("createRandomSetup");
            root.children.Add(toConclusionPlan);
            root.children.Add(toClimacticPlan);
            root.children.Add(createRandomSetup);
        }
        return root;
    }
}
