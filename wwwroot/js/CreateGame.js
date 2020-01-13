var frameCount = 1;
var firstBall = true;
var tenthBallCount = -1;
var gameString = "";
var gameStringRaw = "";
var previousBall = "";
var previousBall2 = "";
var currentBallValue = 10;
var frameScores = new Array(12); //holds score for each frame, frame[1] = score in 1st frame
frameScores[0] = 0; //game starts from 0; could be used to hold handicap if added before game
var gameDone = false; //verify nextBall click is valid
var frames = { //store pins remaining in each frame in a comma-separated list
    1: "",
    2: "",
    3: "",
    4: "",
    5: "",
    6: "",
    7: "",
    8: "",
    9: "",
    10: "",
    11: ""
};
var ballIds = "";
var frameIndex = 0; //misleading name, represents index of the upper portion of the scorecard (scores for each ball)

//pin button onclick function
//Changes buttons color when clicked, also represents change in state where:
//   btn-default = pin not standing
//   btn-dark = pin standing
function pinClicked(clickedId) {
    //if pin is btn-default, change in state means standing, decrease currentBallValue else increase by 1
    currentBallValue += (document.getElementById(clickedId).classList.contains("btn-default")) ? -1 : 1;
    document.getElementById(clickedId).classList.toggle("btn-default");
    document.getElementById(clickedId).classList.toggle("btn-dark");
    document.getElementById(clickedId).classList.toggle("pin-standing");
}

//parses pin list into array of strings, used in nextball function to determine how many pins were left
function parsePins(pins) {
    var res = pins.split(",");
    return res;
}

function formatPins() {
    var res = "";
    for (var i = 1; i < 10; i++) {
        res += frames[i] + ";";
    }
    res += frames[10];
    return res;
}

//handles updating score handling and updating score markers on the UI, called by nextBallClick
function updateScorecard(ballValue) {
    var tframeCount = frameCount - 1; //temporary frameCount to make it more relevant, index of previous frame if 1st ball or current frame if second ball
    var markerId = "marker" + tframeCount; //Id of the marker for frame# tframeCount
    if (frameCount >= 10) {
        if (tenthBallCount == -1) { //second ball of the 10th, must update previous frame if it was a strike and update 10th if open
            if (previousBall2 == "X") {
                //Update previous frame if it was a strike
                //maps / to value of 10, otherwise uses combined values of last 2 balls. Updates frameScores for previous frame
                frameScores[8] = (ballValue == "/") ?
                    frameScores[7] + 20 :
                    frameScores[7] + parseInt(previousBall) + parseInt(ballValue) + 10;
                document.getElementById("marker8").innerHTML = frameScores[8];
            }
            if (ballValue != "X" && ballValue != "/") { //handle open frame
                //set frameScores for current frame to previous frameScore + values of first and second ball
                frameScores[9] = frameScores[8] + parseInt(ballValue) + parseInt(previousBall);
                document.getElementById("marker9").innerHTML = frameScores[9];
            }
        }
        if (tenthBallCount == 0) { //first ball of the tenth, must check if 8th or 9th affected
            if (previousBall == "X" && previousBall2 == "X") { //update 8th if on double
                var value = (ballValue == "X") ? 30 : 20 + parseInt(ballValue);
                frameScores[8] = frameScores[7] + value;
                document.getElementById("marker8").innerHTML = frameScores[8];
            }
            if (previousBall == "/") { //update 9th if spare
                var value = (ballValue == "X") ? 20 : 10 + parseInt(ballValue);
                frameScores[9] = frameScores[8] + value;
                document.getElementById("marker9").innerHTML = frameScores[9];
            }
        }
        if (tenthBallCount == 1) { //second ball of the 10th, must update previous frame if it was a strike and update 10th if open
            if (previousBall2 == "X") { //handle 9th frame if it was a strike
                var valPrev = (previousBall == "X") ? 10 : parseInt(previousBall);
                var valBall = (ballValue == "X") ? 10 : parseInt(ballValue);
                if (ballValue == "/") {
                    valPrev = 0;
                    valBall = 10;
                }
                frameScores[9] = frameScores[8] + valPrev + valBall + 10;
                document.getElementById("marker9").innerHTML = frameScores[9];
            }
            if (previousBall != "X" && ballValue != "/") {
                var score = parseInt(ballValue) + parseInt(previousBall);
                frameScores[10] = frameScores[9] + score;
                document.getElementById("marker10").innerHTML = frameScores[10];
                setGameDone();
            }
        }
        if (tenthBallCount == 2) {
            var sum = 0;
            if (previousBall2 == "X") {
                sum += 10;
                if (previousBall == "X") {
                    sum += 10;
                    sum += (ballValue == "X") ? 10 : parseInt(ballValue);
                } else if (ballValue == "/") {
                    sum += 10;
                } else {
                    sum += parseInt(previousBall) + parseInt(ballValue);
                }
            } else {
                sum += 10;
                sum += (ballValue == "X") ? 10 : parseInt(ballValue);
            }
            frameScores[10] = frameScores[9] + sum;
            document.getElementById("marker10").innerHTML = frameScores[10];
            setGameDone();
        }
        tenthBallCount++;
        return;

    }
    if (!firstBall) { //First ball only requires updates to previous frames, inverted back to what it was at the beginning of nextBallClick call
        if (previousBall == "X" && previousBall2 == "X") { //Update Frame Score For Double
            var tempMarkerId = "marker" + (tframeCount - 1); //temporary value to hold the markerId for 2 frames ago
            //handles parsing of strike to value of 10, adds frame score for 2 frames ago
            frameScores[tframeCount - 1] = (ballValue == "X") ?
                frameScores[tframeCount - 2] + 30 :
                frameScores[tframeCount - 2] + 20 + parseInt(ballValue);
            //sets corresponding score marker on html
            document.getElementById(tempMarkerId).innerHTML = frameScores[tframeCount - 1];
        }
        if (previousBall == '/') { //Update Previous Frame Score for Spare
            //maps X to value of 10 for current frame, sets value of frameScore for previous frame
            frameScores[tframeCount] = (ballValue == "X") ?
                frameScores[tframeCount - 1] + 20 :
                frameScores[tframeCount - 1] + 10 + parseInt(ballValue);
            document.getElementById(markerId).innerHTML = frameScores[tframeCount];
        }
    } else { //second ball requires update to current frame if current ball is not a mark, and previous frame if it was a strike
        if (previousBall2 == "X") {
            //Update previous frame if it was a strike
            var tempMarkerId = "marker" + (tframeCount - 1); //points to previous frames score marker
            //maps / to value of 10, otherwise uses combined values of last 2 balls. Updates frameScores for previous frame
            frameScores[tframeCount - 1] = (ballValue == "/") ?
                frameScores[tframeCount - 2] + 20 :
                frameScores[tframeCount - 2] + parseInt(previousBall) + parseInt(ballValue) + 10;
            document.getElementById(tempMarkerId).innerHTML = frameScores[tframeCount - 1];
        }
        if (ballValue != "X" && ballValue != "/") { //handle open frame
            //set frameScores for current frame to previous frameScore + values of first and second ball
            frameScores[tframeCount] = frameScores[tframeCount - 1] + parseInt(ballValue) + parseInt(previousBall);
            document.getElementById(markerId).innerHTML = frameScores[tframeCount];
        }
    }

}

function setGameDone() {
    document.getElementById("pinsLeftInput").value = formatPins();
    while (gameString.Length < 21) {
        gameString += " ";
    }
    document.getElementById("gameStringInput").value = gameString;
    document.getElementById("scoreInput").value = frameScores[10];
    document.getElementById("notesInput").value = document.getElementById("notes").value;
    document.getElementById("nameInput").value = document.getElementById("name").value;
    document.getElementById("patternInput").value = document.getElementById("pattern").value;
    document.getElementById("gameSubButton").disabled = false;
    ballIds = ballIds.substring(0, ballIds.length - 1);
    console.log(ballIds);
    document.getElementById("ballInput").value = ballIds;
    gameDone = true;
}

//handles when next ball is clicked, performs various actions
//  -deternmines how many and which pin buttons are active
//  -adds pins to frames dictionary
//  -controls styling, disabling and re-enabling of pin buttons
//  -sets ball value in upper portion of scoresheet
//  -calls updateScorecard() to update any necessary scores
function nextBallClick() {
    if (gameDone) { //mitigates index errors that could occur if next ball is clicked after all frames are filled
        return;
    }
    var ballValue = "0"; //holds value to be placed in game template
    if (firstBall) { //handles input for first ball
        var e = document.getElementById("ballDropDown");
        var ballId = e.options[e.selectedIndex].value;
        ballIds += "" + ballId + ",";
        firstBall = false; //toggle first ball to false 
        ballValue = "" + currentBallValue;
        //first ball worth 10 is equal to a strike
        if (ballValue == "10") {
            ballValue = "X";
        } else { //first ball is not a strike, so we must determines which pins were left for the database, and disable buttons for spare
            var framesTemp = ""; //holds the string of pins left for frames
            //disables buttons that were not selected so they cannot be selected on the spare attempt
            //basically, pins cannot be added on the spare attempt
            for (var i = 0; i < document.getElementsByClassName("btn-default").length; i++) {
                if (document.getElementsByClassName("btn-default")[i].classList.contains("pin")) {
                    document.getElementsByClassName("btn-default")[i].disabled = true;
                }
            }
            //loops through pins with btn-dark class (active pins), creates a comma separated list of their ids (numbers)
            var firstIteration = true;
            while (document.getElementsByClassName("pin-standing").length > 0) {
                if (firstIteration) {
                    framesTemp += document.getElementsByClassName("pin-standing")[0].id;
                    firstIteration = false;
                } else {
                    framesTemp += "," + document.getElementsByClassName("pin-standing")[0].id;
                }
                pinClicked(document.getElementsByClassName("pin-standing")[0].id);
            }
            //sets frames value in the dictionary
            if (frames[frameCount] == "") {
                frames[frameCount] = framesTemp;
            }
        }
    } else { //This is the second ball
        var lastBallCount = parsePins(frames[frameCount]).length; //gets the number of pins left in the first ball, probably a better way to do this
        //sets ball value to spare if all pins are knocked down, otherwise sets it to the number of pins with dark (represent fallen pins on second ball)
        ballValue = (currentBallValue == 10) ?
            "/" :
            "" + (lastBallCount - document.getElementsByClassName("pin-standing").length);
        //enables buttons that were disabled for the spare attempt
        for (var i = 0; i < document.getElementsByClassName("btn-default").length; i++) {
            document.getElementsByClassName("btn-default")[i].disabled = false;
        }
        //sets elements with class btn-dark back to btn-default for the next frame
        while (document.getElementsByClassName("pin-standing").length > 0) {
            var element = document.getElementsByClassName("pin-standing")[0].id;
            pinClicked(element);
        }
        firstBall = true; //toggle first ball back to true because second ball is over
        frameCount++; //move to next frame
    }
    //update string representing all frames for the game
    gameString += ballValue;
    gameStringRaw += ballValue;
    updateScorecard(ballValue); //update score markers where necessary
    //updates temporary values used in updateScorecard, should probably be done in updateScorecard
    previousBall2 = previousBall;
    previousBall = ballValue;
    //sets ball score to ballValue for current frame
    document.getElementById("pinsLine").children[frameIndex].innerHTML = ballValue;
    //increase frameIndex (represents ballscore field)
    frameIndex++;
    //set currentBallValue back to default
    currentBallValue = 10;
    //handles blank second ball for strikes for non-10th frames and sets first ball back to true
    if (ballValue == "X" && frameCount < 10) {
        if (frameCount == 9) {
            tenthBallCount++;
        }
        document.getElementById("pinsLine").children[frameIndex].innerHTML = " ";
        gameString += " ";
        frameIndex++;
        frameCount++;
        firstBall = true;
    }
    if (ballValue == "X" && frameCount >= 10) {
        firstBall = true;
    }
}

function DeselectAllClick() {
    for (var i = 0; i < document.getElementsByClassName("pin").length; i++) {
        var element = document.getElementsByClassName("pin")[i].id;
        document.getElementById(element).classList.remove("btn-dark");
        document.getElementById(element).classList.remove("pin-standing");
        document.getElementById(element).classList.add("btn-default");
        currentBallValue = 10;
    }
}

function SelectAllClick() {
    for (var i = 0; i < document.getElementsByClassName("pin").length; i++) {
        if (document.getElementsByClassName("pin")[i].disabled == false) {
            var element = document.getElementsByClassName("pin")[i].id;
            document.getElementById(element).classList.add("btn-dark");
            document.getElementById(element).classList.remove("btn-default");
            document.getElementById(element).classList.add("pin-standing");
        }
        currentBallValue = 0;
    }
}

function deleteClick() {
    if (frameIndex == 0) {
        return;
    }
    if (frameIndex > 18) { //tenth frame
        if (frameIndex == 19) { //deleting first ball of the 10th
            if (document.getElementById("pinsLine").children[14].innerHTML == "X" && document.getElementById("pinsLine").children[16].innerHTML == "X") {
                //if 8th and 9th are strikes must clear frames for both
                document.getElementById("marker8").innerHTML = "";
                document.getElementById("marker9").innerHTML = "";
            } else if (document.getElementById("pinsLine").children[17].innerHTML == "/") {
                //if 9th is a spare its score must be cleared
                document.getElementById("marker9").innerHTML = "";
            }
        } else if (frameIndex == 20) { //deleting second ball of the 10th
            if (document.getElementById("pinsLine").children[16].innerHTML == "X") {
                //if 9th is a strike, must clear score
                document.getElementById("marker9").innerHTML = "";
            }
        }
        document.getElementById("marker10").innerHTML = "";
    } else { //not tenth frame
        if (frameIndex % 2 == 0) { //even frameIndex = deleting second ball of previous frame or strike
            if (frameCount > 1) frameCount--;
            document.getElementById("marker" + frameCount).innerHTML = "";
            if (frameIndex > 3 && gameString[frameIndex - 4] == 'X') {
                document.getElementById("marker" + (frameCount - 1)).innerHTML = "";
                if (frameIndex > 5 && gameString[frameIndex - 6] == 'X' && gameString[frameIndex - 2] == 'X') {
                    document.getElementById("marker" + (frameCount - 2)).innerHTML = "";
                }
            }
            if (gameString[frameIndex - 2] == 'X' && gameString[frameIndex - 3] == '/') {
                document.getElementById("marker" + (frameCount - 1)).innerHTML = "";
            }
            if (gameString[frameIndex - 1] != " ") { // previous frame is not a strike, we must reload pins
                for (var i = 0; i < document.getElementsByClassName("pin").length; i++) {
                    document.getElementsByClassName("pin")[i].disabled = true;
                }
                var framePins = String(frames[frameCount]);
                var framePinsSplit = framePins.split(',');
                for (var i = 0; i < framePinsSplit.length; i++) {
                    document.getElementById(framePinsSplit[i]).disabled = false;
                }
            }
        } else { //odd frameIndex = deleting first ball of current frame
            frames[frameCount] = "";
            for (var i = 0; i < document.getElementsByClassName("pin").length; i++) {
                document.getElementsByClassName("pin")[i].disabled = false;
            }
            if (frameIndex > 2) {
                if (gameString[frameIndex - 2] == '/') {
                    document.getElementById("marker" + (frameCount - 1)).innerHTML = "";
                } else if (frameIndex > 4 && gameString[frameIndex - 3] == 'X' && gameString[frameIndex - 5] == 'X') {
                    document.getElementById("marker" + (frameCount - 2)).innerHTML = "";
                }
            }
        }
        firstBall = !firstBall;
    }
    //handles updating of previousBall and previousBall2
    if (gameStringRaw.length == 1) {
        previousBall = "";
        previousBall2 = "";
    } else if (gameStringRaw.length == 2) {
        previousBall = gameStringRaw[0];
        previousBall2 = "";
    } else {
        previousBall = gameStringRaw[gameStringRaw.length - 2].toString();
        previousBall2 = gameStringRaw[gameStringRaw.length - 3].toString();
    }

    gameStringRaw = gameStringRaw.substring(0, gameStringRaw.length - 1);
    gameString = gameString.substring(0, gameString.length - 1);
    frameIndex--;
    document.getElementById("pinsLine").children[frameIndex].innerHTML = "";
    gameDone = false;
    if (frameIndex > 0 && frameIndex < 18 && document.getElementById("pinsLine").children[frameIndex - 1].innerHTML == "X") {
        frameIndex--;
        document.getElementById("pinsLine").children[frameIndex].innerHTML = "";
        gameString = gameString.substring(0, gameString.length - 1);
        firstBall = true;
    }
}
