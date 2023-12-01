import java.io.BufferedReader;
import java.io.FileReader;
import java.io.IOException;
import java.util.ArrayList;
import java.util.List;

public class Day13 {
    public static void main(String[] args) throws IOException {
        long elapsedTimeSum = 0;
        for (int i = 0; i < 1000; i++) {
            long startTime = System.nanoTime();

            var pairs = parseInput();
            calculate(pairs);

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

    private static void calculate(List<Pair> pairs) {
        int sum = 0;
        for (int i = 0; i < pairs.size(); i++) {
            Pair pair = pairs.get(i);
            if (validateLists(pair.left, pair.right))
                sum += i + 1;
        }
        System.out.println(sum);
    }

    private static boolean validateLists(Array left, Array right) {
        var i = 0;

        while (true) {
            if (i >= left.value.size()) return true;
            if (i >= right.value.size()) return false;

            var leftItem = left.value.get(i);
            var rightItem = right.value.get(i);

            if (!leftItem.isArray() && !rightItem.isArray()) {
                int leftValue = ((Number) leftItem).value;
                int rightValue = ((Number) rightItem).value;
                if (leftValue < rightValue)
                    return true;
                else if (leftValue > rightValue)
                    return false;
            } else if (leftItem.isArray() && rightItem.isArray()) {
                if (!validateLists((Array) leftItem, (Array) rightItem))
                    return false;
            } else {
                if (leftItem.isArray()) {
                    var tempRight = new Array();
                    tempRight.value.add(new Number(((Number) rightItem).value));
                    if (!validateLists((Array) leftItem, tempRight))
                        return false;
                } else {
                    var tempLeft = new Array();
                    tempLeft.value.add(new Number(((Number) leftItem).value));
                    if (!validateLists(tempLeft, (Array) rightItem))
                        return false;
                }
            }

            i++;
        }
    }
}

class Pair {
    Array left;
    Array right;

    public Pair(Array left, Array right) {
        this.left = left;
        this.right = right;
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
    public List<Item> value = new ArrayList<>();
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