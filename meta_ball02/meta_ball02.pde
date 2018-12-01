float C = 0.0;

Ball[] balls;

void setup() {
  size(1000, 800);
  background(255);
  strokeWeight(2);

  balls = new Ball[12];

  balls[0] = new Ball(mouseX, mouseY, 50.0, 100.0);
  balls[1] = new Ball(50.0 * width/100.0, 50.0 * height/100.0, 50.0, 100.0);
  balls[2] = new Ball(62.0 * width/100.0, 50.0 * height/100.0, 50.0, 100.0);
  balls[3] = new Ball(74.0 * width/100.0, 50.0 * height/100.0, 50.0, 100.0);
  balls[4] = new Ball(86.0 * width/100.0, 50.0 * height/100.0, 50.0, 100.0);
  balls[5] = new Ball(38.0 * width/100.0, 50.0 * height/100.0, 50.0, 100.0);
  balls[6] = new Ball(26.0 * width/100.0, 50.0 * height/100.0, 50.0, 100.0);
  balls[7] = new Ball(14.0 * width/100.0, 50.0 * height/100.0, 50.0, 100.0);
  balls[8] = new Ball(25.0 * width/100.0, 80.0 * height/100.0, 80.0, 130.0);
  balls[9] = new Ball(75.0 * width/100.0, 80.0 * height/100.0, 80.0, 130.0);
  balls[10] = new Ball(random(100) * width/100.0, random(100) * height/100.0, 50.0, 100.0, random(-1.0, 1.0), random(-1.0, 1.0));
  balls[11] = new Ball(random(100) * width/100.0, random(100) * height/100.0, 50.0, 100.0, random(-1.0, 1.0), random(-1.0, 1.0));
}

void draw() {
  background(255);

  balls[0].move();

  for (int i = 0; i < height; i++) {
    for (int j = 0; j < width; j++) {
      C = 0.0;
      for (Ball b : balls) {
        C += b.GetC(j, i);
      }

      if (j == mouseX && i == mouseY) {
        println("mouseX = " + mouseX + ", mouseY = " + mouseY);
        println(C);
      }
      if (C >= 1.0) {
        stroke(0, 200, 200);
        point(j, i);
      } else if (C >= 0.9) {
        stroke(0);
        point(j, i);
      }
    }
  }
  for (int i = 0; i < balls.length; i++) {
    balls[i].display();
    if (balls[i].vecx != 0) {
      balls[i].reflection();
    }
  }
}

void mousePressed() {
  println("mouseX = " + mouseX + ", mouseY = " + mouseY);
}

class Ball {
  float posx, posy, rad, te;
  float vecx, vecy, speed;

  Ball(float _posx, float _posy, float _rad, float _te) {
    posx = _posx;
    posy = _posy;
    rad = _rad;
    te = _te;
    vecx = 0;
    vecy = 0;
  }

  Ball(float _posx, float _posy, float _rad, float _te, float _vecx, float _vecy) {
    posx = _posx;
    posy = _posy;
    rad = _rad;
    te = _te;
    vecx = _vecx / sqrt(_vecx * _vecx + _vecy * _vecy);
    vecy = _vecy / sqrt(_vecx * _vecx + _vecy * _vecy);
    speed = 10.0;
  }

  void display() {
    stroke(0, 200, 200);
    fill(0, 200, 200);
    ellipse(posx, posy, rad*2, rad*2);
  }

  void move() {
    posx = mouseX;
    posy = mouseY;
  }

  void reflection() {
    posx += vecx * speed;
    if (posx - rad < 0) {
      posx = rad;
      vecx *= -1;
    }
    if (posx + rad > width) {
      posx = width - rad;
      vecx *= -1;
    }

    posy += vecy * speed;
    if (posy - rad < 0) {
      posy = rad;
      vecy *= -1;
    }
    if (posy + rad > height) {
      posy = height - rad;
      vecy *= -1;
    }
  }

  float GetC(int _x, int _y) {
    float d = dist(posx, posy, _x, _y);
    if (rad <= d && d <= te) {
      return (1.0 / pow(rad - te, 2)) * pow(d - te, 2);
    } else {
      return 0;
    }
  }
}