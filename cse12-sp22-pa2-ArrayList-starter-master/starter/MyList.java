/**
 * This file contains all of the methods for the MyList Interface. All classes
 * implementing this interface should also implement the methods listed here.
 */

/**
 * An interface that specifies the functionality of an ArrayList ADT
 */
public interface MyList<E> {

	void expandCapacity(int requiredCapacity);
	

	int getCapacity();
	

	void insert(int index, E element);
	
	
	void append(E element);
	
	
	void prepend(E element);
	
	
	E get(int index);

	
	E set(int index, E element);
	

	E remove(int index);
	

	int size();
	
}