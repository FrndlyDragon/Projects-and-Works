/**
 * This file contains all the public tests(visible on Gradescope)
 * Use this as a guide to write tests to verify your MyArrayList implementation 
 */

import static org.junit.Assert.*;
import org.junit.*;

/**
 * This class creates a test fixture and runs multiple tests on 
 * your implementation for MyArrayList.  
 * 
 * Instance variables:
 * arr - Object array that will be used to set up the test fixture
 * arrInts - Integer array that will be used to set up the test fixture
 */
public class MyArrayListPublicTester {

	static final int DEFAULT_CAPACITY = 5;
	static final int MY_CAPACITY = 3;

	Object[] arr = new Object[10];
	Integer[] arrInts = { 1, 2, 3 };

	private MyArrayList listEmpty, listDefaultCap, listCustomCapacity, 
		listWithNull, listWithInt;

	/*
	 * This sets up the test fixture. JUnit invokes this method before
	 * every testXXX method. The @Before tag tells JUnit to run this method
	 * before each test
	 */
	@Before
	public void setUp() throws Exception {
		listEmpty = new MyArrayList();
		listDefaultCap = new MyArrayList(DEFAULT_CAPACITY);
		listCustomCapacity = new MyArrayList(MY_CAPACITY);
		listWithNull = new MyArrayList(arr);
		listWithInt = new MyArrayList<Integer>(arrInts);
	}

	// ======================== Public Tests ========================

	/*
	 * Tests Constructor when no arguements are given during instantiation
	 */
	@Test
	public void testBasicConstructor() {
		assertEquals("Check size for default constructor", 0, listEmpty.size);
		assertEquals("Check capacity for default constructor", 5, 
			listEmpty.data.length);

		assertEquals("Check size for the constructor with size argument", 0, 
			listCustomCapacity.size);
		assertEquals("Check capacity for the constructor with size argument", 3,
			listCustomCapacity.data.length);

		assertEquals("Check size for the constructor with list argument", 3,
			listWithInt.size);
		assertEquals("Check capacity for the constructor with size argument", 3, 
			listWithInt.data.length);
	}

	/*
	 * Tests Append method when MyArrayList is not empty
	 */
	@Test
	public void testBasicAppendNonEmpty() {
		listWithNull.append(3);

		assertEquals("Check that append increments size", 11, 
			listWithNull.size);
		assertEquals("Check that capacity is unchanged", 20, 
			listWithNull.data.length);
		assertEquals("check the correct element", null, 
			listWithNull.data[0]);
		assertEquals("check the correct element", 3, 
			listWithNull.data[10]);
	}

	/*
	 * Tests Append method when MyArrayList is empty
	 */
	@Test
	public void testBasicAppendEmpty() {
		listCustomCapacity.append(3);

		assertEquals("Check that append increments size", 1, 
			listCustomCapacity.size);
		assertEquals("Check that capacity is unchanged", 3, 
			listCustomCapacity.data.length);
		assertEquals("check the correct element", 3, 
			listCustomCapacity.data[0]);
	}

	/*
	 * Tests Prepend method when MyArrayList is empty
	 */
	@Test
	public void testBasicPrependEmpty() {
		listDefaultCap.prepend(3);

		assertEquals("Check that prepended item", 3, listDefaultCap.data[0]);
		assertEquals("Check list size after the prepend", 1, 
			listDefaultCap.size);
		assertEquals("Check that capacity is unchanged", 5, 
			listDefaultCap.data.length);
	}

	/*
	 * Tests Prepend method when MyArrayList is not empty
	 */
	@Test
	public void testBasicPrependNonEmpty() {
		listWithInt.prepend(3);

		assertEquals("Check that prepended item", 3, listWithInt.data[0]);
		assertEquals("Check list size after the prepend", 4, listWithInt.size);
		assertEquals("Check that capacity is unchanged", 6, 
			listWithInt.data.length);
	}

	/*
	 * Tests Insert method when an element is inserted at the end
	 */
	@Test
	public void testBasicInsert() {
		listWithNull.insert(0, Integer.valueOf(5));
		listCustomCapacity.insert(0, 100);

		assertEquals("should insert 5 to the list", 5, listWithNull.data[0]);
		assertEquals("should increment size", 11, listWithNull.size);
		assertEquals("capacity of the list should not change when" + 
			"insert one elem", 20, listWithNull.data.length);

		assertEquals("should insert 5 to the list", 100, listCustomCapacity.data[0]);
		assertEquals("should increment size", 1, listCustomCapacity.size);
		assertEquals("capacity of the list should not change when" + 
			"insert one elem", 3, listCustomCapacity.data.length);
	}

	/*
	 * Tests Remove method when an element is removed
	 */
	@Test
	public void testBasicRemove() {
		listWithInt.remove(2);
		assertEquals("The capacity should stay the same", 3, 
			listWithInt.data.length);
		assertEquals("check size after removing an element", 2, 
			listWithInt.size);

		listWithInt.remove(0);
		assertEquals("the capacity should stay the same", 3, 
			listWithInt.data.length);
		assertEquals("check size after removing an element", 1, 
			listWithInt.size);
		assertEquals("check the remaining element", 2, listWithInt.data[0]);

		listWithInt.remove(0);
		assertEquals("the capacity should stay the same", 3, 
			listWithInt.data.length);
		assertEquals("check size after removing an element", 0, 
			listWithInt.size);
	}

	/*
	 * Tests set method to set the correct index to a new value
	 */
	@Test
	public void testBasicSet() {
		listWithNull.set(2, 4);
		assertEquals("Should set index 2 to 4", 4, listWithNull.data[2]);
		assertEquals("Should not change index 0", null, listWithNull.data[0]);
		assertEquals("size should not get incremented", 10, listWithNull.size);
	}

	/*
	 * Tests Get method to return the correct value
	 */
	@Test
	public void testBasicGet() {
		assertEquals("Should get 3 from the list", 3, listWithInt.get(2));
		assertEquals("Should get 2 from the list", 2, listWithInt.get(1));
		assertEquals("Should get 1 from the list", 1, listWithInt.get(0));

		assertEquals("Should get null from the list", null, 
			listWithNull.get(1));
	}

	/*
	 * Tests Size method to return the correct size of MyArrayList
	 */
	@Test
	public void testBasicSize() {
		assertEquals("Check size with empty list", 0, 
			listCustomCapacity.size());
		assertEquals("Check size with non-empty list", 3, listWithInt.size());
		assertEquals("Check size with non-empty list", 10, listWithNull.size());
	}


	/*
	 * Tests expandCapacity method when required capacity
	 * is current capacity + 1
	 */
	@Test
	public void testBasicExpandCapacityDouble() {
		listWithInt.expandCapacity(4);
		assertEquals("Capacity should be updated", 6, listWithInt.data.length);
		assertEquals("The size should still be the same", 3, listWithInt.size);
	}

	/*
	 * Tests if the capacity is explanded to less than 5
	 * it is reset to the default capacity
	 */
	@Test
	public void testBasicExpandCapacityReset() {
		MyArrayList<String> list = new MyArrayList<>();
		String[] origArray = {};
		int origSize = 0;
		list.data = origArray;
		list.expandCapacity(origSize + 1);
		assertEquals("Capacity should be updated", 5, list.data.length);
		assertEquals("The size should still be the same", origSize, list.size);
	}

	/*
	 * Tests getCapacity method to return the correct capacity
	 */
	@Test
	public void testBasicGetCapacity() {
		assertEquals("getCapacity should return the length instance variable data"
			, 10, listWithNull.getCapacity());
		assertEquals("getCapacity should return the length instance variable data"
			, 3, listCustomCapacity.getCapacity());
		assertEquals("getCapacity should return the length instance variable data"
			, 5, listDefaultCap.getCapacity());
	}

}
