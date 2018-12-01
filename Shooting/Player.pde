class Player {
  float posx;
  float posy;
  float wid;
  float hei;
  float speed;
  float vx;
  float vy;
  boolean del;
  int hp;
  int attack;
  int shotNum;
  float hp_s;
  float muteki_s;
  float muteki;

  float shot_limit_s = 0.05; //単位は秒
  float shot_limit = shot_limit_s*60;
  boolean guard;

  Player(float argX, float argY, float argWidth, float argHeight, float argSpeed, int argHp, int argAttack, int argShotNum, float argMuteki) {
    posx = argX;
    posy = argY;
    wid = argWidth;
    hei = argHeight;
    speed = argSpeed;
    del = false;
    hp_s = argHp;
    hp = argHp;
    attack = argAttack;
    shotNum = argShotNum;
    muteki_s = argMuteki;//単位は秒
    muteki = 0;

    guard = false;
  }

  void update() {
    display();
    if (!del) {
      shoot();
      move();
      muteki--;
    }
  }

  void display() {
    fill(34, 180, 34);
    rect(posx, posy, wid, hei);
    if (muteki > 0) {
      fill(0, 100);
      rect(posx, posy, wid, hei);
    }

    //ガード
    if (guard) {
      noFill();
      stroke(34, 180, 34);
      strokeWeight(10);
      ellipse(posx, posy, height/16+30, height/16+30);
      stroke(0);
      strokeWeight(1);
    }
  }

  void move() {
    if (posx + vx + 600/64 > 600 || posx + vx - 600/64 < 0) {
      vx = 0;
    }
    posx += vx;
    if (posy + vy + height/32 > height || posy + vy - height/32 < 0) {
      vy = 0;
    }
    posy += vy;
  }

  void move(boolean argLeft, boolean argRight, boolean argUp, boolean argDown) {
    vx = vy = 0;
    if (argLeft) {
      vx -= speed;
    }
    if (argRight) {
      vx += speed;
    }
    if (argUp) {
      vy -= speed;
    }
    if (argDown) {
      vy += speed;
    }
  }

  //自分のショット処理
  void shoot() {
    shot_limit--;
    if ((keyCode == CONTROL || mousePressed) && !guard) {
      if (shot_limit <= 0  && shots.size() < shotNum) {
        shots.add(new Shot(posx, posy-height/32, 10, attack));
        shot_limit = shot_limit_s*60;
      }
    }
  }

  void be_damaged() {
    if (muteki <= 0 && !guard) {
      hp--;
      if (hp <= 0) {
        del = true;
      }
      muteki = muteki_s*60;
    }
  }
}