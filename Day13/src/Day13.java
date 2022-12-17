import java.io.BufferedReader;
import java.io.FileReader;
import java.io.IOException;
import java.util.ArrayList;
import java.util.LinkedList;
import java.util.List;

public class Day13 {
    public static void main(String[] args) throws IOException {
        long elapsedTimeSum = 0;
        for (int i = 0; i < 1000; i++) {
            long startTime = System.nanoTime();

            var pairs = parseInput();

            long elapsedTime = System.nanoTime() - startTime;
            elapsedTimeSum += elapsedTime;
        }
        System.out.println("average execution time (ns): " + elapsedTimeSum / 1000);
    }

    private static List<Pair> parseInput() throws IOException {
        var input = new java.io.File("inputs/day13.txt");
        var br = new BufferedReader(new FileReader(input));

        String line;
        var pairs = new ArrayList<Pair>();

        while ((line = br.readLine()) != null) {
            var first = parseLine(line);
            var second = parseLine(br.readLine());
            pairs.add(new Pair(first, second));
            br.readLine();
        }

        return pairs;
    }

    private static Array parseLine(String line) {
        int value = 0;
        boolean hasValue = false;

        var array = new Array();
        var current = array;
        line = line.substring(1);

        for (var c : line.toCharArray()) {
            switch (c) {
                case '[':
                    var temp = new Array();
                    temp.parent = current;
                    current.value.add(temp);
                    current = temp;
                    value = 0;
                    hasValue = false;
                    break;
                case ']':
                    if (hasValue) {
                        current.value.add(new Number(value));
                        value = 0;
                        hasValue = false;
                    }
                    current = current.parent;
                    break;
                case ',':
                    if (hasValue) {
                        current.value.add(new Number(value));
                        value = 0;
                        hasValue = false;
                    }
                    break;
                default:
                    hasValue = true;
                    value *= 10;
                    value += c - '0';
                    break;
            }
        }

        return array;
    }
}

class Pair {
    Array first;
    Array second;

    public Pair(Array first, Array second) {
        this.first = first;
        this.second = second;
    }
}

interface Item {
    boolean isArray();
    void print();
}
class Number implements Item {
    public int value;

    public Number(int value) {
        this.value = value;
    }

    @Override
    public boolean isArray() {
        return false;
    }

    @Override
    public void print() {
        System.out.print(value);
    }
}

class Array implements Item {
    public List<Item> value = new LinkedList<>();
    public Array parent;

    @Override
    public boolean isArray() {
        return true;
    }

    @Override
    public void print() {
        System.out.print('[');

        if (!value.isEmpty()) {
            value.get(0).print();
            for (int i = 1; i < value.size(); i++) {
                System.out.print(',');
                value.get(i).print();
            }
        }

        System.out.print(']');
    }
}