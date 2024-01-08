public interface IMenu<T>
{
    void HandleSelection();
    void PrintInfo(List<T> itemList, string header, bool keyContinue = true);
    List<T> GetTimeSlotMenu(string timeSlot);
}