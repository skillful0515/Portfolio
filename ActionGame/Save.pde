class Save {
  float posX, posY;
  float scaleX, scaleY;

  boolean action = false;
  int actionCount = 60;
  int actionCountSpan = 60;

  Save(float _posX, float _posY, float _scaleX, float _scaleY) {
    posX = _posX;
    posY = _posY;
    scaleX = _scaleX;
    scaleY = _scaleY;
  }

  void update() {
    action();
  }

  void display(color _color, color _texColor) {
    fill(_color);
    stroke(0);
    strokeWeight(2);
    rect(posX, posY, scaleX, scaleY);

    fill(_texColor);
    textAlign(CENTER, CENTER);
    textSize(20);
    text("S", posX, posY);
  }

  void action() {
    /*
    if (action == false) {
     display(color(255, 215, 0), 0);
     
     for (Shot s : nowScene.shots) {
     if (abs(posX - s.colPosX) < scaleX/2.0 + s.colScaleX/2.0
     && abs(posY - s.posY) < scaleY/2.0 + s.scaleY/2.0) {
     action = true;
     spawnPosX = playerPosX;
     spawnPosY = playerPosY;
     spawnScene = nowScene;
     }
     }
     
     if (abs(posX - playerPosX) < scaleX/2.0 + playerScaleX/2.0
     && abs(posY - playerPosY) < scaleY/2.0 + playerScaleY/2.0) {
     action = true;
     spawnPosX = playerPosX;
     spawnPosY = playerPosY;
     spawnScene = nowScene;
     }
     }
     if (action == true) {
     display(color(65, 105, 225), 255);
     actionCount--;
     if (actionCount < 0) {
     actionCount = actionCountSpan;
     action = false;
     }
     }
     */

    if (abs(posX - playerPosX) < scaleX/2.0 + playerScaleX/2.0
      && abs(posY - playerPosY) < scaleY/2.0 + playerScaleY/2.0) {
      OnCollider();
    }

    if (action == false) {
      display(color(255, 215, 0), 0);
    }
    if (action == true) {
      display(color(65, 105, 225), 255);
      actionCount--;
      if (actionCount < 0) {
        actionCount = actionCountSpan;
        action = false;
      }
    }
  }

  void OnCollider() {
    action = true;
    spawnPosX = playerPosX;
    spawnPosY = playerPosY;
    spawnScene = nowScene;
    actionCount = actionCountSpan;
  }
}
