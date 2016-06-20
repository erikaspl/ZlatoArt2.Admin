using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZlatoArt2.Admin.Model
{
    public sealed class ObjectIdSerializer : SerializerBase<string>
    {
        public override string Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            var type = context.Reader.GetCurrentBsonType();

            if (type == BsonType.ObjectId)
            {
                return context.Reader.ReadObjectId().ToString();
            }

            var message = string.Format("Cannot convert a {0} to string.", type);
            throw new NotSupportedException(message);
        }

        public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, string value)
        {
            context.Writer.WriteObjectId(new ObjectId(value));
        }
    }
}
