class Door {
  float posX, posY;
  float scaleX, scaleY;

  Scene nextScene;

  // シーン遷移前用変数
  boolean isChanging;
  int changingCount = 60;
  int changingCountSpan = 60;

  int debug;

  Door(float _posX, float _posY, float _scaleX, float _scaleY) {
    posX = _posX;
    posY = _posY;
    scaleX = _scaleX;
    scaleY = _scaleY;
  }

  Door(float _posX, float _posY, float _scaleX, float _scaleY, Scene _nextScene) {
    posX = _posX;
    posY = _posY;
    scaleX = _scaleX;
    scaleY = _scaleY;
    nextScene = _nextScene;
  }

  void update() {
    display();
    action();
    actChanging();
  }

  void display() {
    // 上側の外線用円
    noStroke();
    fill(0);
    ellipse(posX, posY - scaleY/4.0, scaleX + 5, scaleY + 5);

    // 下側の外線用四角
    rect(posX, posY + scaleY/4.0, scaleX + 5, scaleY + 5);

    // 上側の半円
    noStroke();
    fill(255, 99, 71);
    ellipse(posX, posY - scaleY/4.0, scaleX, scaleY);

    // 下側の四角
    rect(posX, posY + scaleY/4.0, scaleX, scaleY);

    // 切れ目の線
    stroke(0);
    strokeWeight(2);
    line(posX, posY - 3*scaleY/4.0, posX, posY + 3*scaleY/4.0);

    // ノブ
    stroke(0);
    strokeWeight(2);
    fill(0, 100);
    ellipse(posX - scaleX/4.5, posY + scaleY/6.0, scaleX/5.0, scaleY/5.0);
    ellipse(posX + scaleX/4.5, posY + scaleY/6.0, scaleX/5.0, scaleY/5.0);
  }

  void action() {
    if (upMove == true && isJumping == false && !cannotMove) {
      if (abs(playerPosX - posX) < playerScaleX/4 + scaleX/4
        && abs(playerPosY - posY) < playerScaleY/2 + scaleY/2) {
        // ドアに入る処理
        isChanging = true;
      }
    }
  }

  void actChanging() {
    if (isChanging == true) {
      cannotMove = true;
      changingCount--;

      noStroke();
      fill(0, (changingCountSpan - changingCount) / (float)changingCountSpan * 255);
      rect(width/2, height/2, width, height);

      if (changingCount <= 0) {
        nowScene = nextScene;
        nowScene.isChanging = true;
        isChanging = false;
        changingCount = changingCountSpan;
        //cannotMove = false;
        return;
      }
    }
  }

  void setScene(Scene _nextScene) {
    nextScene = _nextScene;
  }
}
