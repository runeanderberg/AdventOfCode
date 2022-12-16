import java.awt.*;
import java.io.BufferedReader;
import java.io.FileReader;
import java.io.IOException;
import java.util.LinkedList;

public class Day12 {
    public static void main(String[] args) throws IOException {
        long elapsedTimeSum = 0;
        for (int i = 0; i < 1000; i++) {
            long startTime = System.nanoTime();

            var map = new Map(parseInput());
            calculate(map);

            long elapsedTime = System.nanoTime() - startTime;
            elapsedTimeSum += elapsedTime;
        }
        System.out.println("average execution time (ns): " + elapsedTimeSum / 1000);
    }

    private static char[][] parseInput() throws IOException {
        var input = new java.io.File("inputs/day12.txt");
        var br = new BufferedReader(new FileReader(input));

        return br.lines().map(String::toCharArray).toArray(char[][]::new);
    }

    private static void calculate(Map map) {
        var queue = new LinkedList<Point>();
        queue.add(map.end);
        map.setDistanceTo(map.end.x, map.end.y, 0);

        Point current;
        int x, y;
        while (!queue.isEmpty()) {
            current = queue.pop();

            x = current.x;
            y = current.y;

            if (map.shouldCheck(x, y, x + 1, y)) {
                map.setDistanceTo(x + 1, y, map.getDistanceTo(x, y) + 1);
                queue.add(new Point(x + 1, y));
            }

            if (map.shouldCheck(x, y, x - 1, y)) {
                map.setDistanceTo(x - 1, y, map.getDistanceTo(x, y) + 1);
                queue.add(new Point(x - 1, y));
            }

            if (map.shouldCheck(x, y, x, y + 1)) {
                map.setDistanceTo(x, y + 1, map.getDistanceTo(x, y) + 1);
                queue.add(new Point(x, y + 1));
            }

            if (map.shouldCheck(x, y, x, y - 1)) {
                map.setDistanceTo(x, y - 1, map.getDistanceTo(x, y) + 1);
                queue.add(new Point(x, y - 1));
            }
        }

        System.out.println(map.getDistanceTo(map.start.x, map.start.y));

        int smallest = Integer.MAX_VALUE;
        for (x = 0; x < map.sizeY; x++) {
            for (y = 0; y < map.sizeX; y++) {
                if (map.heightAt(x, y) == 'a' && map.getDistanceTo(x, y) < smallest)
                    smallest = map.getDistanceTo(x, y);
            }
        }

        System.out.println(smallest);
    }
}

class Map {
    private final char[][] heightMap;
    private final int[][] distanceMap;

    public final int sizeX;
    public final int sizeY;

    public Point start;
    public Point end;

    public Map(char[][] heightMap) {
        this.heightMap = heightMap;
        this.sizeX = heightMap.length;
        this.sizeY = heightMap[0].length;
        this.distanceMap = new int[sizeX][sizeY];

        for (int y = 0; y < heightMap.length; y++) {
            for (int x = 0; x < heightMap[0].length; x++) {
                if (heightAt(x, y) == 'S') {
                    start = new Point(x, y);
                } else if (heightAt(x, y) == 'E') {
                    end = new Point(x, y);
                }
                distanceMap[y][x] = Integer.MAX_VALUE;
            }
        }

        this.heightMap[start.y][start.x] = 'a';
        this.heightMap[end.y][end.x] = 'z';
    }

    public char heightAt(int x, int y) {
        return heightMap[y][x];
    }

    public int getDistanceTo(int x, int y) {
        return distanceMap[y][x];
    }

    public void setDistanceTo(int x, int y, int distance) {
        distanceMap[y][x] = distance;
    }

    public boolean traversable(int currentX, int currentY, int targetX, int targetY) {
        return heightDifference(currentX, currentY, targetX, targetY) >= -1;
    }

    public int heightDifference(int x1, int y1, int x2, int y2) {
        return heightAt(x2, y2) - heightAt(x1, y1);
    }

    public boolean validCoordinate(int x, int y) {
        return x >= 0 && y >= 0 && x < sizeY && y < sizeX;
    }

    public boolean shouldCheck(int currentX, int currentY, int targetX, int targetY) {
        return validCoordinate(targetX, targetY)
                && traversable(currentX, currentY, targetX, targetY)
                && getDistanceTo(targetX, targetY) > getDistanceTo(currentX, currentY) + 1;
    }

    public int[][] getDistanceMap() {
        return distanceMap;
    }
}