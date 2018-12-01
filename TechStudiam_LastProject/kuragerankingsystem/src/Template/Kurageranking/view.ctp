<?php
/**
 * @var \App\View\AppView $this
 * @var \App\Model\Entity\Kurageranking $kurageranking
 */
?>
<nav class="large-3 medium-4 columns" id="actions-sidebar">
    <ul class="side-nav">
        <li class="heading"><?= __('Actions') ?></li>
        <li><?= $this->Html->link(__('Edit Kurageranking'), ['action' => 'edit', $kurageranking->Id]) ?> </li>
        <li><?= $this->Form->postLink(__('Delete Kurageranking'), ['action' => 'delete', $kurageranking->Id], ['confirm' => __('Are you sure you want to delete # {0}?', $kurageranking->Id)]) ?> </li>
        <li><?= $this->Html->link(__('List Kurageranking'), ['action' => 'index']) ?> </li>
        <li><?= $this->Html->link(__('New Kurageranking'), ['action' => 'add']) ?> </li>
    </ul>
</nav>
<div class="kurageranking view large-9 medium-8 columns content">
    <h3><?= h($kurageranking->Id) ?></h3>
    <table class="vertical-table">
        <tr>
            <th scope="row"><?= __('Name') ?></th>
            <td><?= h($kurageranking->Name) ?></td>
        </tr>
        <tr>
            <th scope="row"><?= __('Id') ?></th>
            <td><?= $this->Number->format($kurageranking->Id) ?></td>
        </tr>
        <tr>
            <th scope="row"><?= __('Score') ?></th>
            <td><?= $this->Number->format($kurageranking->Score) ?></td>
        </tr>
        <tr>
            <th scope="row"><?= __('Date') ?></th>
            <td><?= h($kurageranking->Date) ?></td>
        </tr>
    </table>
</div>
