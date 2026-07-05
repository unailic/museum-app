using Muzej.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Muzej.Domain.Services
{
    public static class PopustPravila
    {
        public static double IzracunajPopust(TipPosetioca tip) => tip switch
        {
            TipPosetioca.Student => 0.20,
            TipPosetioca.Penzioner => 0.30,
            TipPosetioca.Redovan => 0.0,
            _ => 0.0
        };
    }
}
