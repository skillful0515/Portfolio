class Debris {
  float[] posX, posY;
  float scaleX, scaleY;
  float speed;
  float[] vecX, vecY;
  
  int num = 5;

  boolean del = false;
  int delCount;

  Debris(float _posX, float _posY, float _scaleX, float _scaleY, int _vec) {
    posX = new float[num];
    posY = new float[num];
    vecX = new float[num];
    vecY = new float[num];
    for (int i = 0; i < num; i++) {
      posX[i] = _posX;
      posY[i] = _posY;

      if (_vec == 1) {
        float rand = random(90, 270);
        vecX[i] = cos(rand * PI / 180.0);
        vecY[i] = sin(rand * PI / 180.0);
      } else {
        float rand = random(270, 450);
        vecX[i] = cos(rand * PI / 180.0);
        vecY[i] = sin(rand * PI / 180.0);
      }
    }

    scaleX = _scaleX;
    scaleY = _scaleY;
    speed = 1.0;
  }

  void update() {
    move();
    display();
    checkDelete();
  }

  void move() {
    for (int i = 0; i < num; i++) {
      posX[i] += vecX[i] * speed;
      posY[i] += vecY[i] * speed;
      vecY[i] += 9.8 * 0.01;
    }
  }

  void display() {
    fill(100, 100, 200);
    stroke(0);
    strokeWeight(1);
    for (int i = 0; i < num; i++) {
      rect(posX[i], posY[i], scaleX, scaleY);
    }
  }

  void checkDelete() {
    delCount++;
    if (delCount > 180) {
      del = true;
    }
  }
}
