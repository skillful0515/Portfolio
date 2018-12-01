class Shot {
  float posx;
  float posy;
  float speed;
  int attack;
  PImage bullet;
  float count;

  Shot(float argX, float argY, float argSpeed, int argAttack) {
    posx = argX;
    posy = argY;
    speed = argSpeed;
    attack = argAttack;

    //画像データ読み込み
    bullet = loadImage("images/shot01/shot01_01tra.png");
  }

  void update() {
    display();
    move();
  }

  void display() {
    fill(0, 255, 0);
    count+=10;
    pushMatrix();
    translate(posx, posy);
    rotate(radians(count));
    //ellipse(posx, posy, 10, 10);
    image(bullet, 0, 0, 10, 10);
    popMatrix();
  }

  void move() {
    posy -= speed;
  }

  boolean delete() {
    if (posy <= -height/32) {
      return true;
    } else {
      return false;
    }
  }

  //敵とのあたり判定、敵へのダメージ処理
  boolean hit() {
    //敵との接触
    for (Enemy ene : enemies) {
      if (dist(0, posy, 0, ene.posy) <= 5+ene.hei/2
        && dist(posx, 0, ene.posx, 0) <= 5+ene.wid/2) {
        int value = attack - ene.defense;
        if (value <= 0) {
          value = 1;
        }
        ene.damage(value);

        return true;
      }
    }

    //雑魚との接触
    for (Zako01 zako : zako01) {
      if (dist(0, posy, 0, zako.posy) <= 5+zako.hei/2
        && dist(posx, 0, zako.posx, 0) <= 5+zako.wid/2) {
        int value = attack - zako.defense;
        if (value <= 0) {
          value = 1;
        }
        zako.damage(value);

        return true;
      }
    }

    //ボスとの接触
    for (Boss boss : bosses) {
      if (dist(0, posy, 0, boss.posy) <= 5+boss.hei/2
        && dist(posx, 0, boss.posx, 0) <= 5+boss.wid/2) {
        int value = attack - boss.defense;
        if (value <= 0) {
          value = 1;
        }
        boss.damage(value);

        return true;
      }
    }

    //回復兵との接触
    for (Healing h : healings) {
      if (dist(0, posy, 0, h.posy) <= 5+height/64
        && dist(posx, 0, h.posx, 0) <= 5+600/32) {
        h.damage();
        return true;
      }
    }
    return false;
  }
}

class Shot_ene {
  float posx;
  float posy;
  float speed;

  Shot_ene(float argX, float argY, float argSpeed) {
    posx = argX;
    posy = argY;
    speed = argSpeed;
  }

  void update() {
    display();
    move();
  }

  void display() {
    fill(0, 255, 255);
    ellipse(posx, posy, 10, 10);
  }

  void move() {
    posy += speed;
  }

  boolean delete() {
    if (posy >= height) {
      return true;
    } else {
      return false;
    }
  }

  boolean hit() {
    if (dist(0, posy, 0, player.posy) <= 5+height/32 
      && dist(posx, 0, player.posx, 0) <= 5+600/64) {
      player.be_damaged();
      return true;
    } else {
      return false;
    }
  }
}

class ShotHoming_ene {
  float posx;
  float posy;
  float speed;
  float wid;
  float hei;
  boolean boundMode;

  PVector v;
  float vx;
  float vy;
  PImage bullet; 
  float count;
  int boundCount;

  boolean setNum;

  ShotHoming_ene(float argX, float argY, float argSpeed, float argWidth, float argHeight, boolean argBoundMode) {
    posx = argX;
    posy = argY;
    speed = argSpeed;
    wid = argWidth;
    hei = argHeight;
    boundMode = argBoundMode;

    boundCount = 0;
    setNum = true;

    //画像データを格納
    bullet = loadImage("images/shot03/01.png");
  }

  void update() {
    if (setNum == true) {
      setVel();
      setNum = false;
    }
    display();
    move();
    bounding();
  }

  void display() {
    fill(255, 0, 255);
    count+=45;
    //ellipse(posx, posy, 10, 10);
    pushMatrix();
    translate(posx, posy);
    rotate(radians(count));
    image(bullet, 0, 0, wid, hei);
    popMatrix();
  }

  void move() {
    posx += vx*speed;
    posy += vy*speed;
  }

  boolean delete() {
    if (posy >= height || posy <= 0
      || posx >= 600 || posx <= 0 
      || (dist(posx, posy, player.posx, player.posy) <= wid + height/32+15 && player.guard)) {
      return true;
    } else {
      return false;
    }
  }

  boolean hit() {
    if (dist(0, posy, 0, player.posy) <= wid/8+height/32 
      && dist(posx, 0, player.posx, 0) <= hei/8+600/64) {
      player.be_damaged();
      return true;
    } else {
      return false;
    }
  }

  void setVel() {
    vx = player.posx - posx;
    vy = player.posy - posy;
    v = new PVector(vx, vy);
    v.normalize();
    vx = v.x;
    vy = v.y;
  }

  void bounding() {
    if (boundMode && boundCount < 3) {
      if (posx > 600 - wid/2.0 || posx < wid/2.0) {
        vx *= -1;
        boundCount++;
        if (posx > 600 - wid/2.0) {
          posx = 600 - wid/2.0;
        } else if (posx < wid/2.0) {
          posx = wid/2.0;
        }
      }
      if (posy > height - hei/2.0 || posy < hei/2.0) {
        vy *= -1;
        boundCount++;
        if (posy > height - hei/2.0) {
          posy = height - hei/2.0;
        } else if (posy < hei/2.0) {
          posy = hei/2.0;
        }
      }
    }
  }
}