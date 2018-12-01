<?php
/**
 * @var \App\View\AppView $this
 * @var \App\Model\Entity\Kurageranking[]|\Cake\Collection\CollectionInterface $kurageranking
 */
?>
<nav class="large-3 medium-4 columns" id="actions-sidebar">
    <ul class="side-nav">
        <li class="heading"><?= __('Actions') ?></li>
        <li><?= $this->Html->link(__('New Kurageranking'), ['action' => 'add']) ?></li>
    </ul>
</nav>
<div class="kurageranking index large-9 medium-8 columns content">
    <h3><?= __('Kurageranking') ?></h3>
    <table cellpadding="0" cellspacing="0">
        <thead>
            <tr>
                <th scope="col"><?= $this->Paginator->sort('Id') ?></th>
                <th scope="col"><?= $this->Paginator->sort('Name') ?></th>
                <th scope="col"><?= $this->Paginator->sort('Score') ?></th>
                <th scope="col"><?= $this->Paginator->sort('Date') ?></th>
                <th scope="col" class="actions"><?= __('Actions') ?></th>
            </tr>
        </thead>
        <tbody>
            <?php foreach ($kurageranking as $kurageranking): ?>
            <tr>
                <td><?= $this->Number->format($kurageranking->Id) ?></td>
                <td><?= h($kurageranking->Name) ?></td>
                <td><?= $this->Number->format($kurageranking->Score) ?></td>
                <td><?= h($kurageranking->Date) ?></td>
                <td class="actions">
                    <?= $this->Html->link(__('View'), ['action' => 'view', $kurageranking->Id]) ?>
                    <?= $this->Html->link(__('Edit'), ['action' => 'edit', $kurageranking->Id]) ?>
                    <?= $this->Form->postLink(__('Delete'), ['action' => 'delete', $kurageranking->Id], ['confirm' => __('Are you sure you want to delete # {0}?', $kurageranking->Id)]) ?>
                </td>
            </tr>
            <?php endforeach; ?>
        </tbody>
    </table>
    <div class="paginator">
        <ul class="pagination">
            <?= $this->Paginator->first('<< ' . __('first')) ?>
            <?= $this->Paginator->prev('< ' . __('previous')) ?>
            <?= $this->Paginator->numbers() ?>
            <?= $this->Paginator->next(__('next') . ' >') ?>
            <?= $this->Paginator->last(__('last') . ' >>') ?>
        </ul>
        <p><?= $this->Paginator->counter(['format' => __('Page {{page}} of {{pages}}, showing {{current}} record(s) out of {{count}} total')]) ?></p>
    </div>
</div>
