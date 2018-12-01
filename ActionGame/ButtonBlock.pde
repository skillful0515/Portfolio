class ButtonBlock {
  float posX, posY;
  float scaleX, scaleY;

  boolean action = false;

  boolean isTrap = false;

  ButtonBlock(float _posX, float _posY, float _scaleX, float _scaleY) {
    posX = _posX;
    posY = _posY;
    scaleX = _scaleX;
    scaleY = _scaleY;
  }

  void update() {
    action();
  }

  void display(color _col) {
    if (isTrap == false) {
      stroke(0);
      strokeWeight(1);
      fill(180);
      rect(posX, posY, scaleX, scaleY);
      fill(_col);
      rect(posX, posY, scaleX/2.0, scaleY/2.0);
    } else {
      stroke(0, 100);
      strokeWeight(1);
      noStroke();
      noFill();
      rect(posX, posY, scaleX, scaleY);
    }
  }

  void action() {
    if (action == false) {
      if (abs(posX - playerPosX) < scaleX/2.0 + playerScaleX/2.0
        && abs(posY - playerPosY) < scaleY/2.0 + playerScaleY/2.0) {
        action = true;
        println("Push");
      }
    }

    if (action == false) {
      display(color(255, 0, 0));
    }
    if (action == true) {
      display(color(255, 0, 0, 50));
    }
  }

  void OnCollider() {
    if (action == false) {
      action = true;
      println("Push");
    }
  }
}
