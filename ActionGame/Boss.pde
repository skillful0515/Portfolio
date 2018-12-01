class Boss {
  float posX, posY;
  float scaleX, scaleY;

  int hp = 10;
  int maxHp = 10;
  boolean del = false;

  Boss(float _posX, float _posY, float _scaleX, float _scaleY) {
    posX = _posX;
    posY = _posY;
    scaleX = _scaleX;
    scaleY = _scaleY;
  }

  void update() {
    display();
  }

  void display() {
    // ボス本体
    stroke(0);
    strokeWeight(5);
    fill(100, 0, 0);
    rect(posX, posY, scaleX, scaleY);

    // HPゲージ
    rectMode(CENTER);
    stroke(0);
    strokeWeight(1);
    fill(255, 0);
    rect(width/2.0, 5 * centiHeight, 80 * centiWidth, 5 * centiHeight);

    rectMode(CORNER);
    fill(255, 0, 0, 150);
    rect(10 * centiWidth, 2.5 * centiHeight, (hp / (float)maxHp) * 80 * centiWidth, 5 * centiHeight);
    rectMode(CENTER);
  }

  void damage() {
    hp--;
    if (hp <= 0) {
      del = true;
    }
  }
}
