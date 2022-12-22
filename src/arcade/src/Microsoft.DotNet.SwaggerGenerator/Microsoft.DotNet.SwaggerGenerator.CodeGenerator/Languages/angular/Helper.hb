export class Helper
{
    public static mapValues(obj: Record<string, any>, fn: (v: any) => any): Record<string, any>
    {
        let result: any = {};
        for (const key of Object.keys(obj))
        {
            result[key] = fn(obj[key]);
        }
        return result;
    }
}
