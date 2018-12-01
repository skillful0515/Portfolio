<?php
namespace App\Model\Entity;

use Cake\ORM\Entity;

/**
 * Kurageranking Entity
 *
 * @property int $Id
 * @property string $Name
 * @property int $Score
 * @property \Cake\I18n\FrozenTime $Date
 */
class Kurageranking extends Entity
{

    /**
     * Fields that can be mass assigned using newEntity() or patchEntity().
     *
     * Note that when '*' is set to true, this allows all unspecified fields to
     * be mass assigned. For security purposes, it is advised to set '*' to false
     * (or remove it), and explicitly make individual fields accessible as needed.
     *
     * @var array
     */
    protected $_accessible = [
        'Name' => true,
        'Score' => true,
        'Date' => true
    ];
}
