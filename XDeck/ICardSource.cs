// Något som kort kan dras ifrån: en vanlig kortlek eller en sko.
//Round och Dealer använder det här, så de behöver inte veta vilken det är.
interface ICardSource
{
    Card Draw();
    int Count();
    void ResetIfLow(int threshold);
}