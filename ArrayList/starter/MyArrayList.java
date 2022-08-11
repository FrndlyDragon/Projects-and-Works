import javax.naming.InitialContext;
import javax.naming.ldap.ExtendedRequest;

/**
 * Name: Jordan Huynh
 * ID: A16990643
 * Email: johuynh@ucsd.edu
 * Sources used: None
 * 
 * Creates an ArrayList object that can store objects into a changing array.
 * Methods to manipulate this list are also included.
 */

 /**
  * Implements various methods to create, add, remove, and insert items into an
  * ArrayList. 
  */
public class MyArrayList<E> implements MyList<E> {

    public Object[] data;
    public int size;

    /**
     * Creates an ArrayList with the default settings.
     */
    public MyArrayList() {
        this.data = new Object[5];
        this.size = 0;
    }

    /**
     * Creates an ArrayList with the capacity set at a given integer.
     * 
     * @param initialCapacity Determines the capacity of the ArrayList
     */
    public MyArrayList(int initialCapacity) {
        if (initialCapacity < 0) {
            throw new IllegalArgumentException();
        }
        this.data = new Object[initialCapacity];
        this.size = 0;
    }

    /**
     * Creates an ArrayList with the items and capacity of a given array.
     * 
     * @param arr Array that is being copied.
     */
    public MyArrayList(E[] arr) {
        if (arr == null) {
            this.data = new Object[5];
        }
        else {
            this.data = arr;
            this.size = arr.length;
        }
    }

    /**
     * Increases the capacity of an ArrayList. Doubles the capacity if the
     * required capacity is less than or equal to double the current capacity.
     * Sets capacity to whatever the required capacity is if it's more than
     * double.
     * 
     * @param requiredCapacity The capacity that the ArrayList needs to have.
     */
    public void expandCapacity(int requiredCapacity){
        if (requiredCapacity < this.data.length) {
            throw new IllegalArgumentException();
        }
        else {
            Object[] temp = new Object[data.length];

            //Copies data into temporary array
            for (int i = 0; i < temp.length; i++) {
                temp[i] = data[i];
            }

            if (data.length == 0) {
                this.data = new Object[5];
            }

            else if (requiredCapacity <= data.length * 2) {
                this.data = new Object[data.length * 2];
            }

            else {
                this.data = new Object[requiredCapacity];
            }

            for (int i = 0; i < temp.length; i++) {
                data[i] = temp[i];
            }
        }
    }

    /**
     * Retrieves the capacity of the ArrayList
     * 
     * @return length of the array
     */
    public int getCapacity() {
        return this.data.length;
    }

    /**
     * Inserts a value at a given point of the ArrayList. Shifts elements if 
     * needed and expands the ArrayList if needed.
     * 
     * @param index Where the element is going to be put
     * @param element Element that is being inserted
     */
    public void insert(int index, E element) {
        if (index >= this.data.length || index < 0) {
            throw new IndexOutOfBoundsException();
        }

        if (this.data.length == size) {
            expandCapacity(this.data.length + 1);
        }

        //We can just shift everything to the right starting from the end of
        //array since the last object will be null.
        for (int i = this.data.length - 1; i > index; i--) {
            this.data[i] = this.data[i - 1];
        }

        this.data[index] = element;
        size += 1;
    }

    /**
     * Places element after all other elements in the ArrayList. If needed, 
     * the array will be expanded.
     * 
     * @param element Element being placed
     */
    public void append(E element) {
        if (size == this.data.length) {
            expandCapacity(this.data.length + 1);
        }

        this.data[size] = element;

        size += 1;
    }

    /**
     * Places element at front of the ArrayList. If needed, the array will be 
     * expanded
     * 
     * @param element Element being placed.
     */
    public void prepend(E element) {
        if (size == this.data.length) {
            expandCapacity(this.data.length + 1);
        }
        for (int i = this.data.length - 1; i > 0; i--) {
            this.data[i] = this.data[i - 1];
        }

        this.data[0] = element;
        size += 1;
    }

    /**
     * Gets the element at a given index of the ArrayList
     * 
     * @param index Where in the ArrayList the element is being pulled from
     * @return Returns the element at the index
     */
    public E get(int index) {
        if (index >= this.data.length || index < 0) {
            throw new IndexOutOfBoundsException();
        }

        return (E)this.data[index];
    }

    /**
     * Sets the element at a given index to a new element.
     * 
     * @param index Index of the element that is being replaced
     * @param element Element that is going to be put into the ArrayList
     * @return Returns the element that was overwritten
     */
    public E set(int index, E element) {
        if (index >= this.data.length || index < 0) {
            throw new IndexOutOfBoundsException();
        }

        E overwritten_element = (E)this.data[index];
        this.data[index] = element;

        return overwritten_element;
    }

    /**
     * Removes an element from the ArrayList and shifts the elements in the 
     * ArrayList to fill that spot.
     * 
     * @param index Index of the element that is being removed
     * @return Element that was removed
     */
    public E remove(int index) {
        if (index >= this.data.length || index < 0) {
            throw new IndexOutOfBoundsException();
        }

        E removed_element = (E)this.data[index];
        for (int i = index; i < size; i++) {
            if (i + 1 >= size) {
                this.data[i] = null;
            }
            else {
                this.data[i] = this.data[i + 1];
            }
        }

        size -= 1;
        return removed_element;
    }

    /**
     * Gets the amount of elements in the ArrayList
     * 
     * @return Size variable
     */
    public int size() {
        return this.size;
    }
}
