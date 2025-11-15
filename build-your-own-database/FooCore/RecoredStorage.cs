namespace FooCore
{
    /// <summary>
    /// Record storage service that store data in form of records, each
    /// record made up from one or several blocks
    /// </summary>
    public class RecoredStorage : IRecoredStorage
    {
        readonly IBlockStorage storage;

        const int MaxRecordSize = 4194304; // 4MB
        const int kNextBlockId = 0;
        const int kRecordLength = 1;
        const int kBlockContentLength = 2;
        const int kPreviousBlockId = 3;
        const int kIsDeleted = 4;

        //
        // Constructors
        //

        public RecoredStorage(IBlockStorage storage)
        {
            ArgumentNullException.ThrowIfNull(storage);

            this.storage = storage;

            if (storage.BlockHeaderSize < 48)
                throw new ArgumentException("Record storage needs at least 48 header bytes");
        }

        //
        // Public Methods
        //

        public byte[] Find(uint recordId)
        {
            throw new NotImplementedException();
        }

        public uint Create(Func<uint, byte[]> dataGenerator)
        {
            throw new NotImplementedException();
        }

        public uint Create(byte[] data)
        {
            throw new NotImplementedException();
        }

        public virtual uint Create()
        {
            using (var firstBlock = AllocateBlock())
            {
                return firstBlock.Id;
            }
        }

        public void Delete(uint recordId)
        {
            throw new NotImplementedException();
        }

        public void Update(uint recordId, byte[] data)
        {
            throw new NotImplementedException();
        }

        //
        // Private Methods
        //

        /// <summary>
        /// Find all blocks of given record, return these blocks in order.
        /// </summary>
        /// <param name="recordId">Record identifier.</param>
        List<IBlock> FindBlocks(uint recordId)
        {
            var blocks = new List<IBlock>();
            var success = false;

            try
            {
                var currentBlockId = recordId;

                do
                {
                    // Grab next block
                    var block = storage.Find(currentBlockId);
                    if (null == block)
                    {
                        // Special case: if block #0 never created, then attempt to create it
                        if (currentBlockId == 0)
                        {
                            block = storage.CreateNew();
                        }
                        else
                        {
                            throw new Exception("Block not found by id: " + currentBlockId);
                        }
                    }
                    blocks.Add(block);

                    // If this is a deleted block then ignore the fuck out of it
                    if (1L == block.GetHeader(kIsDeleted))
                    {
                        throw new InvalidDataException("Block not found: " + currentBlockId);
                    }

                    // Move next
                    currentBlockId = (uint)block.GetHeader(kNextBlockId);
                } while (currentBlockId != 0);

                success = true;
                return blocks;
            }
            finally
            {
                // Incase shit happens, dispose all fetched blocks
                if (false == success)
                {
                    foreach (var block in blocks)
                    {
                        block.Dispose();
                    }
                }
            }
        }

        /// <summary>
        /// Allocate new block for use, either by dequeuing an existing non-used block
        /// or create a new one
        /// </summary>
        /// <returns>Newly allocted block ready to use.</returns>
        IBlock AllocateBlock()
        {
            uint reusableBlockId;
            IBlock newBlock;
            if (false == TryFindFreeBlock(out reusableBlockId)) { }
            return null;
        }

        bool TryFindFreeBlock(out uint blockId)
        {
            blockId = 0;
            GetSpaceTrackingBlock(out IBlock lastBlock, out IBlock secondLastBlock);

            using (lastBlock)
            using (secondLastBlock)
            {
                // If this block is empty, then goto previous block
                var currentBlockContentLength = lastBlock.GetHeader(kBlockContentLength);
                if (currentBlockContentLength == 0)
                {
                    // If there is no previous block, return false to indicate we can't dequeue
                    if (secondLastBlock == null)
                    {
                        return false;
                    }

                    // Dequeue an uint from previous block, then mark current block as free
                    blockId = ReadUInt32FromTrailingContent(secondLastBlock);
                }
            }

            return false;
        }

        uint ReadUInt32FromTrailingContent(IBlock block)
        {
            var buffer = new byte[4];
            var contentLength = block.GetHeader(kBlockContentLength);

            if ((contentLength % 4) != 0)
            {
                throw new DataMisalignedException("Block content length not %4: " + contentLength);
            }

            if (contentLength == 0)
            {
                throw new InvalidDataException("Trying to dequeue UInt32 from an empty block");
            }

            block.Read(dst: buffer, dstOffset: 0, srcOffset: (int)contentLength - 4, count: 4);
            return 0;
            // return LittleEndianByteOrder.GetUInt32(buffer);
        }

        /// <summary>
        /// Get the last 2 blocks from the free space traking record.
        /// </summary>
        void GetSpaceTrackingBlock(out IBlock lastBlock, out IBlock secondLastBlock)
        {
            lastBlock = null;
            secondLastBlock = null;

            // Grab all record 0's blocks
            var blocks = FindBlocks(0);
            try
            {
                if (blocks == null || (blocks.Count == 0))
                {
                    throw new Exception("Failed to find blocks of record 0");
                }

                // Assign
                lastBlock = blocks[blocks.Count - 1];
                if (blocks.Count > 1)
                {
                    secondLastBlock = blocks[blocks.Count - 2];
                }
            }
            finally
            {
                // Always dispose unused blocks
                if (blocks != null)
                {
                    foreach (var block in blocks)
                    {
                        if (
                            (lastBlock == null || block != lastBlock)
                            && (secondLastBlock == null || block != secondLastBlock)
                        )
                        {
                            block.Dispose();
                        }
                    }
                }
            }
        }
    }
}
