class Shot {
  float posX, posY;
  float scaleX, scaleY;
  float speed;
  int vec;

  float befPosX, befPosY;
  float colPosX, colPosY;
  float colScaleX, colScaleY;

  boolean del = false;

  Shot(float _posX, float _posY, float _scaleX, float _scaleY, float _speed, int _vec) {
    posX = _posX;
    posY = _posY;
    scaleX = _scaleX;
    scaleY = _scaleY;
    speed = _speed;
    vec = _vec;

    befPosX = _posX;
    befPosY = _posY;
    colPosX = _posX;
    colPosY = _posY;
    colScaleX = _scaleX;
    colScaleY = _scaleY;
  }

  void update() {
    move();
    action();
    display();
  }

  void display() {
    stroke(0);
    strokeWeight(1);
    fill(0, 0, 200);
    rect(posX, posY, scaleX, scaleY);
  }

  void move() {
    befPosX = posX;

    posX += vec * speed;
  }

  void action() {
    colPosX = (befPosX + posX + vec * scaleX) / 2.0;
    colScaleX = abs(posX - befPosX);

    // あたり判定用

/*
    stroke(1);
    fill(255);
    rect(colPosX, posY, colScaleX, scaleY);
*/

    for (Block b : nowScene.blocks) {
      if (abs(colPosX - b.posX) < colScaleX/2.0 + b.scaleX/2.0
        && abs(posY - b.posY) < scaleY/2.0 + b.scaleY/2.0) {
        del = true;
        nowScene.addDebris(new Debris(b.posX - vec * b.scaleX / 2.0, posY, 3.0, 3.0, vec));
      }
    }

    for (Boss b : nowScene.bosses) {
      if (abs(colPosX - b.posX) < colScaleX/2.0 + b.scaleX/2.0
        && abs(posY - b.posY) < scaleY/2.0 + b.scaleY/2.0) {
        b.damage();
        del = true;
      }
    }

    for (Save s : nowScene.saves) {
      if (abs(colPosX - s.posX) < colScaleX/2.0 + s.scaleX/2.0
        && abs(posY - s.posY) < scaleY/2.0 + s.scaleY/2.0) {
        s.OnCollider();
      }
    }

    for (ButtonBlock b : nowScene.buttonBlocks) {
      if (b.isTrap == false) {
        if (abs(colPosX - b.posX) < colScaleX/2.0 + b.scaleX/2.0
          && abs(posY - b.posY) < scaleY/2.0 + b.scaleY/2.0) {
          b.OnCollider();
        }
      }
    }
  }
}
