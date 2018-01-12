﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UNote.Services.External.Dto
{
    /// <summary>
    /// 企业微信接口默认响应
    /// </summary>
    public class CorpWeixinResponseDto
    {
        public CorpWeixinResponseDto()
        {
            errcode = 404;
        }

        public int errcode { get; set; }

        public string errmsg { get; set; }

        public bool IsSuccess()
        {
            return errcode == 0;
        }
    }
}
